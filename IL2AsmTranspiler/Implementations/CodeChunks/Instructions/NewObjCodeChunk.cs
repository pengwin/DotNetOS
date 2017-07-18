using System.Reflection;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class NewObjCodeChunk : ICodeChunk
    {
        private readonly int _sizeInBytes;

        private readonly IMethodCodeChunk _mallocChunk;

        private readonly IMethodCodeChunk _constructorChunk;

        public NewObjCodeChunk(int sizeInBytes, IMethodCodeChunk mallocChunk, IMethodCodeChunk constructorChunk)
        {
            _sizeInBytes = sizeInBytes;
            _mallocChunk = mallocChunk;
            _constructorChunk = constructorChunk;
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create($"mov eax, {_sizeInBytes} ; size in bytes to allocate",
                $"call {_mallocChunk.Label} ; call runtime allocate memory",
                "push eax ;store pointer to object memory",
                $"call {_constructorChunk.Label} ; constructor");
        }

        public IMnemonicsStream Code => GetCode();
    }
}
