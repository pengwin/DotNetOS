namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class StindI4CodeChunk : BaseInstructionChunk
    {
        public StindI4CodeChunk() : base(
                "pop ebx ; val",
                "pop eax ; address",
                "mov [eax], ebx")
        {
        }
    }
}
