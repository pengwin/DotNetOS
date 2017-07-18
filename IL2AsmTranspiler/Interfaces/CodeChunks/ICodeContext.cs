using System;
using System.Reflection;
using Common;

namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface ICodeContext : ICodeChunk
    {
        Option<IRuntimeDefinition> RuntimeDefinition { get; }

        Option<IMethodCodeChunk> ResolveMethod(MethodInfo method);

        Option<ITypeCodeChunk> ResolveType(Type type);

        Option<IStaticFieldCodeChunk> ResolveStaticField(FieldInfo field);

        Option<IFieldCodeChunk> ResolveField(FieldInfo field);

        IInernedStringCodeChunk StringIntern(string stringToIntern);

        string DefaultConstructorsLabel { get; }
    }
}
