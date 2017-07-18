using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class BaseAddressChunk : ICodeChunk
    {

        private readonly int _baseAddress;

        public BaseAddressChunk(int baseAddress)
        {
            _baseAddress = baseAddress;
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create($"org 0x{_baseAddress:X}");
        }

        public IMnemonicsStream Code { get; }
    }
}
