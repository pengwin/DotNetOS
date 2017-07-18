using System;
using System.Threading.Tasks;

namespace ToolsRunner.Interfaces
{
    public interface IFasmRunner
    {
        Task<IFasmResult> CompileAsync(string fileName, string outputFile);
    }
}