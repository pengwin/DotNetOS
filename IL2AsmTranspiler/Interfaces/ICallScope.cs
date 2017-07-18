using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Interfaces
{
    public interface ICallScope
    {
        ICodeContext CodeContext { get; }
    }
}
