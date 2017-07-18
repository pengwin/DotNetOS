using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.Interfaces
{
    public interface ISourceBuilder
    {
        ISourceBuilder AddChunk(ICodeChunk chunk);

        string Build();
    }
}
