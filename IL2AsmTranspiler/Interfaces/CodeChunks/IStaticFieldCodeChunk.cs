namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface IStaticFieldCodeChunk : ICodeChunk
    {
        string Label { get; }
    }
}
