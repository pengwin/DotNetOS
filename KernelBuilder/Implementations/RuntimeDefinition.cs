using System;
using System.Reflection;
using IL2AsmTranspiler.Interfaces;

namespace KernelBuilder.Implementations
{
    internal class RuntimeDefinition : IRuntimeDefinition
    {
        public RuntimeDefinition(Type runtime)
        {
            Memset = runtime.GetMethod("MemSet");
        }

        public string HeapLabel { get; }
        public MethodInfo Memset { get; }
    }
}