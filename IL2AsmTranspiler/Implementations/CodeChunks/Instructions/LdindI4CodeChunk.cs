namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdindI4CodeChunk : BaseInstructionChunk
    {
        public LdindI4CodeChunk() : base(
                "pop eax ; address",
                "mov ebx, [eax]",
                "push ebx")
        {
        }
    }
}
