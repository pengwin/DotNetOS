using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using IL2AsmTranspiler.Interfaces.Factories;

namespace IL2AsmTranspiler.Implementations.Factories
{
    internal class DecompilerFactory : IDecompilerFactory
    {
        private readonly IComponentContext _context;

        public DecompilerFactory(IComponentContext context)
        {
            _context = context;
        }
        public IDecompiler GetDecompiler(ICodeContext codeContext)
        {
            return _context.Resolve<IDecompiler>(new TypedParameter(typeof (ICodeContext), codeContext));
        }
    }
}
