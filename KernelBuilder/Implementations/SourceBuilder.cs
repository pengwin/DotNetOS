using System;
using System.Collections.Generic;
using System.Linq;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using KernelBuilder.Interfaces;

namespace KernelBuilder.Implementations
{
    internal class SourceBuilder : ISourceBuilder
    {
        private readonly IList<ICodeChunk> _chunks;

        public SourceBuilder()
        {
            _chunks = new List<ICodeChunk>();
        }

        public ISourceBuilder AddChunk(ICodeChunk chunk)
        {
            _chunks.Add(chunk);
            return this;
        }

        public string Build()
        {
            return string.Join(Environment.NewLine, _chunks.Select(x => x.Code));
        }
    }
}
