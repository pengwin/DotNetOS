namespace IL2AsmTranspiler.Interfaces.CodeChunks
{
    public interface IMethodCodeChunk : ICodeChunk
    {
        string Name { get; }
        string Label { get; }
        int ParametersCount { get; }
        IMnemonicsStream GetMethodEpilogue();
        int GetLocalVariableOffset(int localIndex);
        int GetArgumentOffset(int argIndex);
        bool HasReturnValue { get; }
    }
}
