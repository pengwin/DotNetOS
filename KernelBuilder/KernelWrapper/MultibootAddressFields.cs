using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class MultibootAddressFields : ICodeChunk
    {
        public string HeaderLabel { get; set; }

        public string LoadStartLabel { get; set; }

        public string LoadEndLabel { get; set; }

        public string BssEndLabel { get; set; }

        public string EntryLabel { get; set; }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"\t dd {HeaderLabel} ; header start",
                $"\t dd {LoadStartLabel} ; load start",
                $"\t dd {LoadEndLabel} ; load end",
                $"\t dd {BssEndLabel} ; bss end",
                $"\t dd {EntryLabel} ; entry");
        }

        public IMnemonicsStream Code => GetCode();
    }
}