using System.Linq;
using System.Text;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks
{
    internal class InernedStringCodeChunk : IInernedStringCodeChunk
    {
        public InernedStringCodeChunk(string label, string stringToIntern)
        {
            Label = label;
            var asciiSymbols = Encoding.ASCII.GetBytes(stringToIntern).Select(x => $"0x{x:X}");
            Code = MnemonicStreamFactory.Create($"{label}:", $"dd {stringToIntern.Length}", 
            $"db {string.Join(",", asciiSymbols)}",
            "db 0"); // null terminator
        }

        public IMnemonicsStream Code { get; }
        public string Label { get; }
    }
}
