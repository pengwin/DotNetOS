namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class BrTrueSCodeChunk : BaseInstructionChunk
    {
        public BrTrueSCodeChunk(string targetLabel) : base(
            "pop eax", //value 1
            "cmp eax, 1",
            $"je {targetLabel}"
            ) 
        {
        }
    }
}
