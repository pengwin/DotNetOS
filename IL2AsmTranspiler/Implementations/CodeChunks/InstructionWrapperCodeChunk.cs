using System;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks
{
    internal class InstructionWrapperCodeChunk : ICodeChunk
    {

        public InstructionWrapperCodeChunk(string label, ICodeChunk instructionCode, string instructionComment)
        {
            Code = MnemonicStreamFactory.Create($"{label}: ;{instructionComment}", instructionCode.Code);
        }

        public IMnemonicsStream Code { get; }
    }
}
