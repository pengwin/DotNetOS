using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using ToolsRunner.Interfaces;

namespace ToolsRunner.Implementations
{
    internal class QemuTestBuilder : IQemuTestBuilder
    {
        #region Private fields

        private const int ThreadPauseMs = 200;

        private readonly string _qemuGreetings;

        private readonly string _qemuCommand;

        private readonly string _biosPath;

        private readonly int _maxAttemptCount;

        private readonly int _memSizeMegs;

        private readonly ILogger _logger;

        private CommandRunner _qemu;

        private readonly IList<Func<Task>> _flow = new List<Func<Task>>();

        private Option<Action<string>> _failAction = Option<Action<string>>.None;

        #endregion Private fields

        #region Constructor

        public QemuTestBuilder(ILogger logger, IQemuRunnerSettings settings)
        {
            _qemuGreetings = $"QEMU {settings.QemuVersion} monitor - type 'help' for more information";
            _qemuCommand = settings.QemuExecutablePath;
            _biosPath = settings.BiosPath;
            _memSizeMegs = settings.MemorySizeMegs;
            _maxAttemptCount = (settings.TimeoutS*1000/ThreadPauseMs);
            _logger = logger;
        }

        #endregion Constructor

        #region Public methods

        public IQemuTestBuilder Start(string kernel, Option<Action<string>> failAction)
        {
            _failAction = failAction;
            var pathToBios = System.IO.Path.Combine(_biosPath, "bios.bin");
            var args = new[] {"-monitor stdio", $"-L {_biosPath}", $"-bios {pathToBios}", $"-kernel {kernel}", $"-m {_memSizeMegs}" };
            _flow.Add(() => StartProcessAsync(args));
            return this;
        }

        public IQemuTestBuilder Stop()
        {
            _flow.Add(() => StopAsync(ThreadPauseMs * 2));
            _flow.Add(() => PrintErrorOutputAsync());
            return this;
        }

        public IQemuTestBuilder WaitForAddressValue(UInt32 address, UInt32 value)
        {
            _flow.Add(() => WaitForAddressValueAsync(address, value, _failAction));
            return this;
        }

        public IQemuTestBuilder WaitForRegisterValue(string register, UInt32 value)
        {
            _flow.Add(() => WaitForRegisterValueAsync(register, value, _failAction));
            return this;
        }

        public IQemuTestBuilder PrintDebugMesage(string message)
        {
            _flow.Add(() => PrintDebugMesageAsync(message));
            return this;
        }

        public async Task RunAsync()
        {
            foreach (var task in _flow)
            {
                await task();
            }
        }

        #endregion Public methods

        #region Private methods

        #region Async wrappers

        private void InternalPrintErrorMesage(string message)
        {
            _logger.Error("Qemu", message);
        }

        private async Task PrintDebugMesageAsync(string message)
        {
            _logger.Debug("Qemu", message);
            await Task.FromResult(true);
        }

        private async Task PrintErrorOutputAsync()
        {
            var errorOutput = await _qemu.ToErrorOutputAsync();
            _logger.Error("Qemu", errorOutput);
            await Task.FromResult(true);
        }

        private async Task WaitForRegisterValueAsync(string register, UInt32 value, Option<Action<string>> failAction)
        {
            var adressFormatted = $"print ${register}";
            var valueFormatted = $"0x{value.ToString("X").ToLower()}";
            await WaitCommandResult(adressFormatted, valueFormatted, failAction);
        }

        private async Task WaitForAddressValueAsync(UInt32 address, UInt32 value, Option<Action<string>> failAction)
        {
            var adressFormatted = $"xp /1dwx 0x{address.ToString("X").ToLower()}";
            var valueFormatted = $"{address.ToString("X16").ToLower()}: 0x{value.ToString("X8").ToLower()}";
            await WaitCommandResult(adressFormatted, valueFormatted, failAction);
        }

        private async Task StopAsync(int timeoutMs)
        {
            await _qemu.SendCommandAsync("q");
            await _qemu.WaitForEndAsync(timeoutMs);
            await PrintDebugMesageAsync("Qemu ended");
        }

        #endregion Async wrappers

        private async Task StartProcessAsync(string[] arguments)
        {
            _qemu = new CommandRunner();
            _qemu.Start(_qemuCommand, arguments);
            await WaitLastOutputLine(_qemuGreetings, Option<Action<string>>.None);
            await PrintDebugMesageAsync("Qemu started");
        }

        private async Task WaitLastOutputLine(string lineContent, Option<Action<string>> failAction)
        {
            await WaitForAction(async () => await CheckOutputLineAsync(lineContent, false), $"{lineContent}", failAction, Option<Func<Task>>.None);
        }

        private async Task WaitCommandResult(string command, string result, Option<Action<string>> failAction)
        {
            await _qemu.SendCommandAsync(command);
            await WaitForAction(async () => await CheckOutputLineAsync(result), $"{command} == {result}", failAction, Option<Func<Task>>.New(async () => await _qemu.SendCommandAsync(command)));
        }

        private async Task WaitForAction(Func<Task<bool>> condition, string waitDescription, Option<Action<string>> failActionFunc, Option<Func<Task>> beforeAction)
        {
            var attemptCount = 0;
            while (!await condition())
            {
                if (attemptCount > _maxAttemptCount)
                {
                    await Assert(failActionFunc, $"Wait for {waitDescription} is too long");
                }
                if (!beforeAction.IsNone)
                {
                    await beforeAction.Value();
                }
                await Task.Delay(ThreadPauseMs);
                attemptCount++;
            }
        }

        private async Task<Option<string>> ReadOutputLineAsync(bool skipQemuPrompt = true)
        {
            var lastLine = _qemu.ReadOutputLine();
            if (lastLine.IsNone)
            {
                return lastLine;
            }
            if (skipQemuPrompt && lastLine.Value.StartsWith("(qemu)"))
            {
                lastLine = _qemu.ReadOutputLine();
            }
            await PrintDebugMesageAsync($"Qemu output: {lastLine.Value}");
            return lastLine;
        }

        private async Task<bool> CheckOutputLineAsync(string lineToMatch, bool skipQemuPrompt = true)
        {
            var line = await ReadOutputLineAsync(skipQemuPrompt);
            if (line.IsNone)
            {
                return false;
            }

            return line.Value == lineToMatch;
        }

        private async Task Assert(Option<Action<string>> failAction, string message)
        {
            if (failAction.IsNone)
            {
                return;
            }
            await PrintStateInfo();
            await StopAsync(ThreadPauseMs*2);
            await PrintErrorOutputAsync();
            failAction.Value(message);
        }

        private async Task PrintStateInfo()
        {
            /*InternalPrintErrorMesage("EIP Data (20 instructions)");
            await _qemu.SendCommandAsync("xp /20i $eip");
            await Task.Delay(ThreadPauseMs);
            InternalPrintErrorMesage(string.Join(Environment.NewLine, _qemu.ReadAllOutputLines().Where(x => !x.StartsWith("(qemu)"))));
            InternalPrintErrorMesage("ESP Data (20 words)");
            await _qemu.SendCommandAsync("xp /20wx $esp");
            await Task.Delay(ThreadPauseMs);
            InternalPrintErrorMesage(string.Join(Environment.NewLine, _qemu.ReadAllOutputLines().Where(x => !x.StartsWith("(qemu)"))));*/
        }

        #endregion Private methods

        public void Dispose()
        {
            if (_qemu != null)
            {
                _qemu.Dispose();
                _qemu = null;
            }
        }
    }
}
