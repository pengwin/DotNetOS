namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class DivCodeChunk : BaseInstructionChunk
    {
        public DivCodeChunk() : base(
                "pop eax",
                "pop ebx",
                "div ebx",
                "push eax")
        {
        }
    }
}
