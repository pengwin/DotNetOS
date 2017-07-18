using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks
{
    public class BaseIntrinsicChunk : IIntrinsicChunk
    {
        public BaseIntrinsicChunk(string label, params string[] mnemonics)
        {
            Label = label;
            Code = MnemonicStreamFactory.Create(
                $";Intrinsic for '{Label}' begin",
                mnemonics,
                $";Intrinsic for '{Label}' end");
        }

        public string Label { get; }
        public IMnemonicsStream Code { get; }
    }
}
