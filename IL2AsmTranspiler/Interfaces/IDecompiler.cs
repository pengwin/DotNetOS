using System;
using System.Collections.Generic;
using System.Reflection;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Interfaces
{
    public interface IDecompiler
    {
        ITypeCodeChunk GetTypeBody(Type type);
        IMethodCodeChunk GetMethodBody(MethodInfo method);
    }
}