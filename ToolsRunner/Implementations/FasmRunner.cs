using System;
using System.Threading.Tasks;
using Common.Interfaces;
using ToolsRunner.Interfaces;

namespace ToolsRunner.Implementations
{
    internal class FasmRunner : IFasmRunner
    {
        private readonly string _fasmCommand;

        private readonly int _timeoutMs;

        private readonly ILogger _logger;

        public FasmRunner(ILogger logger, IFasmRunnerSettings settings)
        {
            _fasmCommand = settings.FasmExecutablePath;
            _timeoutMs = settings.TimeoutS*1000;
            _logger = logger;
        }

        public async Task<IFasmResult> CompileAsync(string fileName, string outputFile)
        {
            using (var fasm = new CommandRunner())
            {
                fasm.Start(_fasmCommand, new[] { fileName, outputFile });
                await fasm.WaitForEndAsync(_timeoutMs);
                var compilerOutput = string.Join(Environment.NewLine, fasm.ReadAllOutputLines());
                var errorOutput = await fasm.ToErrorOutputAsync();
                _logger.Debug("Fasm", compilerOutput);
                if (!string.IsNullOrEmpty(errorOutput))
                {
                    _logger.Error("Fasm", errorOutput);
                }
                return new FasmResult(errorOutput, compilerOutput);
            }   
        }
    }
}
