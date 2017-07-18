namespace ToolsRunner.Interfaces
{
    public interface IFasmRunnerSettings
    {
        string FasmExecutablePath { get; }

        int TimeoutS { get; }
    }
}