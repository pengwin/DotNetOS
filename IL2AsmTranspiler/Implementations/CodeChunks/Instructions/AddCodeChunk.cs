namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class AddCodeChunk : BaseInstructionChunk
    {
        public AddCodeChunk() : base(
                "pop eax",
                "pop ebx",
                "add eax,ebx",
                "push eax")
        {
        }
    }
}
