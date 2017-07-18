using System;
using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class EntryChunk : ICodeChunk
    {
        private const string EntryMethodName = "Entry";

        private readonly string _stackLabel;
        public string EntryLabel => "_entry";

        private readonly string _kernelEntry;

        private readonly string _defaultConstructorsLabel;

        public EntryChunk(ITypeCodeChunk sources, string stackLabel, string defaultConstructorsLabel)
        {
            var entryMethod = sources.GetMethod(EntryMethodName);
            if (entryMethod.IsNone)
            {
                throw new ArgumentException($"Method {EntryMethodName} is not found in type {sources.Label}");
            }
            _kernelEntry = entryMethod.Value.Label;
            _stackLabel = stackLabel;
            _defaultConstructorsLabel = defaultConstructorsLabel;
            Code = GetCode();
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"{EntryLabel}:",
                $"mov esp, {_stackLabel} ;setup stack",
                $"call {_defaultConstructorsLabel}",
                $"call {_kernelEntry}",
                "mov [0x7C00], word 0xEFD",
                $"hlt ; halt processor",
                $"jmp 1-$ ;jump to previous"
                );
        }

        public IMnemonicsStream Code { get; }
    }
}
