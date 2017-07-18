using System.Reflection;

namespace IL2AsmTranspiler.Interfaces
{
    public interface IRuntimeDefinition
    {
        string HeapLabel { get; }

        MethodInfo Memset { get; }
    }
}
