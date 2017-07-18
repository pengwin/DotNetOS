using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class KernelEndChunk : ICodeChunk
    {
        private readonly string _endLabel;

        private readonly string _bssLabel;

        private readonly string _bssEndLabel;

        private readonly string _applicationSources;

        public KernelEndChunk(string applicationSources, string endLabel, string bssLabel, string bssEndLabel)
        {
            _endLabel = endLabel;
            _bssLabel = bssLabel;
            _bssEndLabel = bssEndLabel;
            _applicationSources = applicationSources;
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"{_endLabel}:",
                $"{_applicationSources}",
                $"{_bssLabel}:",
                "dd 0",
                $"{_bssEndLabel}:");
        }

        public IMnemonicsStream Code { get; }
    }
}
