namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class LdstrCodeChunk : BaseInstructionChunk
    {
        public LdstrCodeChunk(string stringLabel) : base($"mov eax, {stringLabel}", "push eax")
        {
        }
    }
}
