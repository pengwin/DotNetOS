namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class CeqCodeChunk : BaseInstructionChunk
    {
        public CeqCodeChunk(string label, string globalLabel) : base(
            "pop ebx", //value2
            "pop eax", //value 1
            "cmp eax, ebx",
            $"jne {globalLabel}_not_equal",
            "mov eax, 1",
            "push eax",
            $"jmp {globalLabel}_end_ceq",
            $"{label}_not_equal:",
            "mov eax, 0",
            "push eax",
            $"{label}_end_ceq:"
            ) 
        {
        }
    }
}
