using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdsFldCodeChunk : BaseInstructionChunk
    {
        public LdsFldCodeChunk(IStaticFieldCodeChunk staticField) : base($"mov ebx, {staticField.Label}", "mov eax, [ebx]", "push eax")
        {
        }
    }
}
