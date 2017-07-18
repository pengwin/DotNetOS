using System;
using System.Linq;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal abstract class BaseInstructionChunk : ICodeChunk
    {
        protected BaseInstructionChunk(string code)
        {
            Code = MnemonicStreamFactory.Create(code);
        }

        protected BaseInstructionChunk(params string[] codes)
        {
            Code = MnemonicStreamFactory.Create(codes);
        }

        public IMnemonicsStream Code { get; }
    }
}
