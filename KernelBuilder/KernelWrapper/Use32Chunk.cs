using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class Use32Chunk : ICodeChunk
    {
        public Use32Chunk()
        {
            Code = MnemonicStreamFactory.Create("use32");
        }

        public IMnemonicsStream Code { get; }
    }
}
