using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Interfaces.Factories
{
    public interface IDecompilerFactory
    {
        IDecompiler GetDecompiler(ICodeContext codeContext);
    }
}
