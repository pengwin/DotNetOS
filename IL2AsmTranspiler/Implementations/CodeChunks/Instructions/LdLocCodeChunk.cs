using System;
using System.Reflection;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdLocCodeChunk : ICodeChunk
    {
        private readonly int _localOffset;
        public LdLocCodeChunk(int localIndex, IMethodCodeChunk methodChunk)
        {
            _localOffset = methodChunk.GetLocalVariableOffset(localIndex);
            Code = GetCode();
        }

        public LdLocCodeChunk(LocalVariableInfo localVariable, IMethodCodeChunk methodChunk)
        {
            _localOffset = methodChunk.GetLocalVariableOffset(localVariable.LocalIndex);
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"mov eax, [ebp+({_localOffset})]",
                "push eax");
        }

        public IMnemonicsStream Code { get; }
    }
}
