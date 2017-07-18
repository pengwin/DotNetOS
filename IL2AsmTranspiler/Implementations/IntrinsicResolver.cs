using System.Reflection;
using Autofac;
using Common;
using IL2AsmTranspiler.Extensions;
using IL2AsmTranspiler.Implementations.CodeChunks;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using Intrinsic;

namespace IL2AsmTranspiler.Implementations
{
    internal class IntrinsicResolver : IIntrinsicResolver
    {

        private Option<IIntrinsicChunk> Resolve(IntrinsicAttribute intrinsicAttribute, string methodLabel)
        {
            var chunk = new BaseIntrinsicChunk(methodLabel, intrinsicAttribute.Mnemonics);
            return Option<IIntrinsicChunk>.New(chunk);
        }

        public Option<IIntrinsicChunk> Resolve(MethodInfo method)
        {
            var intrinsicAttribute = method.GetCustomAttribute<IntrinsicAttribute>();
            if (intrinsicAttribute == null)
            {
                return Option<IIntrinsicChunk>.None;
            }
            return Resolve(intrinsicAttribute, method.GetMethodLabel());
        }
    }
}