using System;
using System.Reflection;
using Common;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Interfaces
{
    interface IIntrinsicResolver
    {
        Option<IIntrinsicChunk> Resolve(MethodInfo method);
    }
}
