namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StindI2CodeChunk : BaseInstructionChunk
    {
        public StindI2CodeChunk() : base(
                "pop ebx ; val",
                "pop eax ; address",
                "mov [eax], bx")
        {
        }
    }
}
