using System;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations
{
    internal class FieldCodeChunk : IFieldCodeChunk
    {
        public FieldCodeChunk(int offset)
        {
            Code = MnemonicStreamFactory.Empty;
            Offset = offset;
        }

        public IMnemonicsStream Code { get; }
        public int Offset { get; }
    }
}
