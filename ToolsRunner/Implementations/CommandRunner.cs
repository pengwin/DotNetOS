using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common;
using ToolsRunner.Extensions;
using ToolsRunner.Interfaces;

namespace ToolsRunner.Implementations
{
    internal class CommandRunner : ICommandRunner
    {

        #region Private fields

        private ConcurrentQueue<string> _outputQueue;  

        private Process _process;

        #endregion Private fields

        #region Constuctor

        public CommandRunner()
        {
        }

        #endregion Constuctor

        #region Public methods

        public void Start(string command, string[] commandParams)
        {
            _outputQueue = new ConcurrentQueue<string>();
            _process = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = string.Join(" ", commandParams)
                }
            };
            _process.Start();
            _process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data == null)
                {
                    return;
                }
                _outputQueue.Enqueue(args.Data);
            };
            _process.BeginOutputReadLine();
        }

        public async Task SendCommandAsync(string command)
        {
            CheckProcess();

            await _process.StandardInput.WriteLineAsync(command);
            await _process.StandardInput.FlushAsync();
        }

        public Option<string> ReadOutputLine()
        {
            CheckProcess();
            string result;
            if (!_outputQueue.TryDequeue(out result))
            {
                return Option<string>.None;
            }

            return Option<string>.New(result);
        }

        public IEnumerable<string> ReadAllOutputLines()
        {
            CheckProcess();
            var result = ReadOutputLine();
            while (!result.IsNone)
            {
                yield return result.Value;
                result = ReadOutputLine();
            }
        }

        public async Task WaitForEndAsync(int timeoutMs)
        {
            CheckProcess();
            await WaitForExitAsync().TimeoutAfter(timeoutMs);
        }

        public void Stop()
        {
            CheckProcess();
            _process.Kill();
        }

        public async Task<string> ToErrorOutputAsync()
        {
            CheckProcess();
            return await _process.StandardError.ReadToEndAsync();
        }

        #endregion Public methods

        #region Private methods

        private void CheckProcess()
        {
            if (_process == null)
            {
                throw new Exception("Process is not running");
            }
        }

        public Task WaitForExitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<object>();
            _process.EnableRaisingEvents = true;
            _process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(tcs.SetCanceled);
            }
            
            return tcs.Task;
        }

        #endregion Private methods

        #region IDisposable implementation 

        public void Dispose()
        {
            if (_process != null)
            {
                if (!_process.HasExited)
                {
                    _process.Kill();
                }
                _process.Dispose();
                _process = null;
            }
        }

        #endregion IDisposable implementation
    }
}
