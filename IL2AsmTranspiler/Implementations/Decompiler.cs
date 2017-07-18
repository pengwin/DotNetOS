using System;
using System.Reflection;
using IL2AsmTranspiler.Implementations.CodeChunks;
using IL2AsmTranspiler.Implementations.CodeChunks.Instructions;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using IL2AsmTranspiler.Interfaces.Factories;

namespace IL2AsmTranspiler.Implementations
{
    internal class Decompiler : IDecompiler
    {
        private readonly IInstructionConverter _converterFactory;

        public Decompiler(ICodeContext codeContext, IInstructionConverterFactory converterFactory)
        {
            _converterFactory = converterFactory.GetConverter(codeContext);
        }

        public IMethodCodeChunk GetMethodBody(MethodInfo method)
        {
            return new MethodCodeChunk(method, _converterFactory);
        }

        public ITypeCodeChunk GetTypeBody(Type type)
        {
            return new TypeCodeChunk(type, _converterFactory);
        }
    }
}
