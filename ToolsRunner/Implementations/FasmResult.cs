using ToolsRunner.Interfaces;

namespace ToolsRunner.Implementations
{
    internal class FasmResult : IFasmResult
    {
        public string ErrorResult { get; private set; }
        public string Output { get; private set; }

        public FasmResult(string errorResult, string output)
        {
            ErrorResult = errorResult;
            Output = output;
        }
    }
}