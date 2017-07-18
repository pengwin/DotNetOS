using IL2AsmTranspiler.Interfaces.CodeChunks;
using Mono.Reflection;

namespace IL2AsmTranspiler.Interfaces
{
    public interface IInstructionConverter
    {
        ICodeChunk Convert(Instruction instruction, IMethodCodeChunk currentMethodChunk);
    }
}