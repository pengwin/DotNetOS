using System;
using System.Reflection;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StLocCodeChunk : ICodeChunk
    {
        private readonly int _localOffset;
        public StLocCodeChunk(int localIndex, IMethodCodeChunk methodChunk)
        {
            _localOffset = methodChunk.GetLocalVariableOffset(localIndex);
            Code = GetCode();
        }

        public StLocCodeChunk(LocalVariableInfo localVariable, IMethodCodeChunk methodChunk)
        {
            _localOffset = methodChunk.GetLocalVariableOffset(localVariable.LocalIndex);
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                "pop eax",
                $"mov [ebp+({_localOffset})], eax");
        }

        public IMnemonicsStream Code { get; }
    }
}
