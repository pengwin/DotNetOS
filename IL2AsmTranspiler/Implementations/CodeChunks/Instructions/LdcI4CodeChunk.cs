using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdcI4CodeChunk : ICodeChunk
    {
        private readonly int _value;
        public LdcI4CodeChunk(int value)
        {
            _value = value;
            Code = GetCode();
        }
        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create($"mov eax, 0x{_value:X}", "push eax");
        }

        public IMnemonicsStream Code { get; }
    }
}
