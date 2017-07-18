using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class HeapChunk : ICodeChunk
    {
        public int HeapSizeBytes { get; set; }

        public IMnemonicsStream Code => MnemonicStreamFactory.Create($"__heap_top: rb {HeapSizeBytes}");
    }
}
