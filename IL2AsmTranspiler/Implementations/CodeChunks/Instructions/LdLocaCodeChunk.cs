using System;
using System.Reflection;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdLocaCodeChunk : ICodeChunk
    {
        private readonly int _localOffset;
        public LdLocaCodeChunk(LocalVariableInfo localVariable, IMethodCodeChunk methodChunk)
        {
            _localOffset = methodChunk.GetLocalVariableOffset(localVariable.LocalIndex);
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                "mov eax, ebp",
                _localOffset > 0 ? $"add eax, {Math.Abs(_localOffset)}" : $"sub eax, {Math.Abs(_localOffset)}", // local var address
                "push eax");
        }

        public IMnemonicsStream Code { get; }
    }
}
