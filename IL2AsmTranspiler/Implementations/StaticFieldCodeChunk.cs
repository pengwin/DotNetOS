using System;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Implementations
{
    internal class StaticFieldCodeChunk : IStaticFieldCodeChunk
    {
        public StaticFieldCodeChunk(string label, Type fieldType)
        {
            Label = label;
            Code = MnemonicStreamFactory.Create($"{label}: {GetFieldSize(fieldType)} 0x0");
        }

        public IMnemonicsStream Code { get; }
        public string Label { get; }

        private string GetFieldSize(Type fieldType)
        {
            if (fieldType.IsClass || fieldType.IsInterface || fieldType == typeof(uint) || fieldType == typeof(string))
            {
                return "dd";
            }

            if ( fieldType == typeof(byte) || fieldType == typeof(sbyte))
            {
                return "db";
            }

            if (fieldType == typeof(short) || fieldType == typeof(ushort))
            {
                return "dw";
            }

            throw new ArgumentException($"Unknown type {fieldType}", nameof(fieldType));
        }
    }
}
