using System.Runtime.InteropServices;
using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class MultibootHeaderChunk : ICodeChunk
    {
        private readonly MultibootHeaderInfo _info;

        public MultibootHeaderChunk(MultibootHeaderInfo info)
        {
            _info = info;
            Code = GetCode();
        }
        private IMnemonicsStream GetCode()
        {
            var addressFields = _info.Flags.HasFlag(MultibootHeaderInfo.HeaderFlags.UseMultibootAddressFields)
                ? _info.AddressFields.Code
                : MnemonicStreamFactory.Empty;
            var vidInfo = _info.Flags.HasFlag(MultibootHeaderInfo.HeaderFlags.VideoMode)
                ? _info.VideoModeInfo.Code
                : MnemonicStreamFactory.Empty;

            return MnemonicStreamFactory.Create(
                $"{_info.AddressFields.HeaderLabel}:",
                _info.Code,
                addressFields,
                vidInfo);
        }

        public IMnemonicsStream Code { get; }
    }
}
