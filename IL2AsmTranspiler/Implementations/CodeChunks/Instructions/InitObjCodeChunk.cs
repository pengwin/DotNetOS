using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations.CodeChunks.Instructions
{
    internal class InitObjCodeChunk : BaseInstructionChunk
    {
        public InitObjCodeChunk(int size, IMethodCodeChunk memset) : base("pop eax",
                                                "push eax", // src
                                                $"push dword 0xff", //value
                                                $"push dword 0x{size:X}", //sizeInBytes
                                                $"call {memset.Label}")
        {
        }
    }
}
