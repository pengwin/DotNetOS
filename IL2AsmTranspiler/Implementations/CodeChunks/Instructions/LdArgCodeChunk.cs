using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdArgCodeChunk : ICodeChunk
    {
        private readonly int _argOffset;
        public LdArgCodeChunk(int argIndex, IMethodCodeChunk methodChunk)
        {
            _argOffset = methodChunk.GetArgumentOffset(argIndex);
            Code = GetCode();
        }
        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create($"mov eax, [ebp+({_argOffset})]", "push eax");
        }

        public IMnemonicsStream Code { get; }
    }
}
