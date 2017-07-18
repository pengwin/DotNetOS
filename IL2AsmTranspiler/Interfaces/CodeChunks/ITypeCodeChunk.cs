using System.Reflection;
using Common;

namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface ITypeCodeChunk : ICodeChunk
    {
        Option<IMethodCodeChunk> GetMethod(string name);

        Option<IMethodCodeChunk> GetMethod(MethodInfo method);

        Option<IStaticFieldCodeChunk> GetStaticField(FieldInfo field);

        Option<IFieldCodeChunk> GetField(FieldInfo field);

        Option<IMethodCodeChunk> DefaultStaticConstructor { get; } 

        string Label { get; }


    }
}
