using System.Reflection;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StArgSCodeChunk : ICodeChunk
    {
        private readonly int _argOffset;
        public StArgSCodeChunk(ParameterInfo arg, IMethodCodeChunk methodChunk)
        {
            _argOffset = methodChunk.GetArgumentOffset(arg.Position);
            Code = GetCode();
        }
        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create($"pop eax", $"mov [ebp+({_argOffset})], eax");
        }

        public IMnemonicsStream Code { get; }
    }
}
