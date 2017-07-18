using System.Collections.Generic;

namespace IL2AsmTranspiler.Interfaces
{
    public interface IMnemonicsStream : IEnumerable<string>
    {
        string ToString();
    }
}
