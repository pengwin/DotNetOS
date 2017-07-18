using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StsFldCodeChunk : BaseInstructionChunk
    {
        public StsFldCodeChunk(IStaticFieldCodeChunk staticField) : base("pop ebx", $"mov eax, {staticField.Label}", "mov [eax], ebx")
        {
        }
    }
}
