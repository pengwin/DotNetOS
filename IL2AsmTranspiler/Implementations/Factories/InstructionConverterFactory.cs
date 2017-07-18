using Autofac;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using IL2AsmTranspiler.Interfaces.Factories;

namespace IL2AsmTranspiler.Implementations.Factories
{
    internal class InstructionConverterFactory : IInstructionConverterFactory
    {
        private readonly IComponentContext _context;

        public InstructionConverterFactory(IComponentContext context)
        {
            _context = context;
        }

        public IInstructionConverter GetConverter(ICodeContext codeContext)
        {
            return _context.Resolve<IInstructionConverter>(new TypedParameter(typeof(ICodeContext), codeContext));
        }
    }
}
