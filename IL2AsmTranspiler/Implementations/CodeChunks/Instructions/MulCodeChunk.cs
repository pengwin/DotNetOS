namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class MulCodeChunk : BaseInstructionChunk
    {
        public MulCodeChunk() : base(
                "pop eax",
                "pop ebx",
                "mul ebx",
                "push eax")
        {
        }
    }
}
