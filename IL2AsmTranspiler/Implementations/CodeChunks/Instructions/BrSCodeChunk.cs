namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class BrSCodeChunk : BaseInstructionChunk
    {
        public BrSCodeChunk(string label) : base($"jmp {label}")
        {
        }
    }
}
