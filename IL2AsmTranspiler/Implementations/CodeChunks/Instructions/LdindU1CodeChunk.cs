namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdindU1CodeChunk : BaseInstructionChunk
    {
        public LdindU1CodeChunk() : base(
                "pop eax ; address",
                "xor ebx, ebx",
                "mov bl, [eax]",
                "push ebx")
        {
        }
    }
}
