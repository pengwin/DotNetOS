using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface IInernedStringCodeChunk : ICodeChunk
    {
        string Label { get; }
    }
}
