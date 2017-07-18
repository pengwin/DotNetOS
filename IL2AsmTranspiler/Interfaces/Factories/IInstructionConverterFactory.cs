using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace IL2AsmTranspiler.Interfaces.Factories
{
    public interface IInstructionConverterFactory
    {
        IInstructionConverter GetConverter(ICodeContext codeContext);
    }
}
