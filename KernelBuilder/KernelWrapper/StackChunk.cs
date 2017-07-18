using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class StackChunk : ICodeChunk
    {
        private readonly int _sizeInBytes;

        private readonly string _stackLabel;
        public StackChunk(int sizeInBytes, string stackLabel)
        {
            _sizeInBytes = sizeInBytes;
            _stackLabel = stackLabel;
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create($"_someStack: rb {_sizeInBytes}", $"{_stackLabel}:");
        }

        public IMnemonicsStream Code { get; }
    }
}
