namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StindI1CodeChunk : BaseInstructionChunk
    {
        public StindI1CodeChunk() : base(
                "pop ebx ; val",
                "pop eax ; address",
                "mov [eax], bl")
        {
        }
    }
}
