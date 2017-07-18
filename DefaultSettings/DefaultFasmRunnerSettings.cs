using ToolsRunner.Interfaces;

namespace DefaultSettings
{
    public class DefaultFasmRunnerSettings : IFasmRunnerSettings
    {
        public string FasmExecutablePath => @"C:\Coding\osdev\fasm\fasm.exe";
        public int TimeoutS => 1;
    }
}
