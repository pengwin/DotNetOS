namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class BrFalseSCodeChunk : BaseInstructionChunk
    {
        public BrFalseSCodeChunk(string targetLabel) : base(
            "pop eax", //value 1
            "cmp eax, 0",
            $"je {targetLabel}"
            ) 
        {
        }
    }
}
