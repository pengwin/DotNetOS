using Common;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Interfaces.Factories
{
    public interface ICodeContextFactory
    {
        ICodeContext GetCodeContext(Option<IRuntimeDefinition> runtime);
    }
}
