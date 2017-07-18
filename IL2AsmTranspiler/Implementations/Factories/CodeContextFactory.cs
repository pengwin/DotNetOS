using Common;
using IL2AsmTranspiler.Implementations.CodeChunks;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using IL2AsmTranspiler.Interfaces.Factories;

namespace IL2AsmTranspiler.Implementations.Factories
{
    internal class CodeContextFactory : ICodeContextFactory
    {

        private readonly IDecompilerFactory _decompilerFactory;

        public CodeContextFactory(IDecompilerFactory decompilerFactory)
        {
            _decompilerFactory = decompilerFactory;
        }

        public ICodeContext GetCodeContext(Option<IRuntimeDefinition> runtime)
        {
            return new CodeContext(_decompilerFactory, runtime);
        }
    }
}
