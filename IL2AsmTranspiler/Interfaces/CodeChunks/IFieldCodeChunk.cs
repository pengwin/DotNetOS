namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface IFieldCodeChunk : ICodeChunk
    {
        int Offset { get; }
    }
}
