namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface IIntrinsicChunk : ICodeChunk
    {
        string Label { get; }
    }
}
