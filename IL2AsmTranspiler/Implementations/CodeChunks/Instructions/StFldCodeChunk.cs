using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StFldCodeChunk : BaseInstructionChunk
    {
        public StFldCodeChunk(IFieldCodeChunk staticField) : base("pop ebx", // value
                                                                  "pop eax", //object address
                                                                  $"mov [eax+{staticField.Offset}], ebx")
        {
        }
    }
}
