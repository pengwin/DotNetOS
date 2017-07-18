namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class SubCodeChunk : BaseInstructionChunk
    {
        public SubCodeChunk() : base(
                "pop ebx",
                "pop eax",
                "sub eax,ebx",
                "push eax")
        {
        }
    }
}
