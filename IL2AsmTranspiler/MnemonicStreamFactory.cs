using IL2AsmTranspiler.Implementations;
using IL2AsmTranspiler.Interfaces;

namespace IL2AsmTranspiler
{
    public static class MnemonicStreamFactory
    {
        public static IMnemonicsStream Create(params object[] mnemonics) => new MnemonicsStream(mnemonics);

        public static IMnemonicsStream Create(params string[] mnemonics) => new MnemonicsStream(mnemonics);

        public static IMnemonicsStream Empty => new MnemonicsStream();
    }
}
