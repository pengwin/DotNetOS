using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdFldCodeChunk : BaseInstructionChunk
    {
        public LdFldCodeChunk(IFieldCodeChunk field) : base("pop eax", $"mov ebx, [eax+{field.Offset}]", "push ebx")
        {
        }
    }
}
