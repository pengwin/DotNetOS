namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class CltCodeChunk : BaseInstructionChunk
    {
        public CltCodeChunk(string label, string globalLabel) : base(
            "pop ebx", //value2
            "pop eax", //value 1
            "cmp eax, ebx",
            $"jl {globalLabel}_less",
            "mov eax, 0",
            "push eax",
            $"jmp {globalLabel}_else",
            $"{label}_less:",
            "mov eax, 1",
            "push eax",
            $"{label}_else:"
            ) 
        {
        }
    }
}
