using System;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class RetInstructionChunk : ICodeChunk
    {
        private readonly IMethodCodeChunk _methodChunk;

        public RetInstructionChunk(IMethodCodeChunk methodChunk)
        {
            _methodChunk = methodChunk;
            Code = GetCode();
        }
        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(_methodChunk.GetMethodEpilogue(), "ret");
        }

        public IMnemonicsStream Code { get; }
    }
}
