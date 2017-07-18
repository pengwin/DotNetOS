using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations
{
    internal class MnemonicsStream : IMnemonicsStream
    {
        private readonly IEnumerable<string> _mnemonics;

        public MnemonicsStream(params object[] mnemonics)
        {
            _mnemonics = mnemonics.SelectMany(ParamToEnumerable).ToArray();
        }

        public override string ToString() => string.Join(Environment.NewLine, _mnemonics);
        public IEnumerator<string> GetEnumerator()
        {
            return _mnemonics.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<string> ParamToEnumerable(object param)
        {
            var stringParam = param as string;
            if (stringParam != null)
            {
                return new[] {stringParam};
            }

            var mnemonicsCollectionParam = param as IEnumerable<IMnemonicsStream>;
            if (mnemonicsCollectionParam != null)
            {
                return mnemonicsCollectionParam.SelectMany(x => x);
            }

            var stringCollectionParam = param as IEnumerable<string>;
            if (stringCollectionParam != null)
            {
                return stringCollectionParam;
            }

            var codeChunk = param as ICodeChunk;
            if (codeChunk != null)
            {
                return codeChunk.Code;
            }

            throw new ArgumentException($"Wrong param type {param.GetType()}", nameof(param));
        } 
    }
}
