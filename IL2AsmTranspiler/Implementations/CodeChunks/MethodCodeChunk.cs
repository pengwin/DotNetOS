using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using IL2AsmTranspiler.Extensions;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using Mono.Reflection;

namespace IL2AsmTranspiler.Implementations.CodeChunks
{
    internal class MethodCodeChunk : IMethodCodeChunk
    {
        private readonly IEnumerable<ICodeChunk> _chunks;

        public string Name { get; }
        public string Label { get; }

        private readonly IList<LocalVariableInfo> _localVariables;
        public int ParametersCount { get; }

        public MethodCodeChunk(MethodInfo method, IInstructionConverter converter)
        {
            Name = method.Name;
            Label = method.GetMethodLabel();
            var body = method.GetMethodBody();
            if (body == null)
            {
                throw new ArgumentException($"Method {Label} doesn't have body");
            }
            _localVariables = body.LocalVariables;
            ParametersCount = method.GetParameters().Length;
            HasReturnValue = method.ReturnType != typeof(void);
            _chunks = method.GetInstructions().Select(i => converter.Convert(i, this));
            Code = GetCode();  
        }

        public MethodCodeChunk(ConstructorInfo constructor, IInstructionConverter converter)
        {
            Name = constructor.Name;
            Label = constructor.GetConstructorLabel();
            var body = constructor.GetMethodBody();
            if (body == null)
            {
                throw new ArgumentException($"Constructor {Label} doesn't have body");
            }
            _localVariables = body.LocalVariables;
            ParametersCount = constructor.GetParameters().Length;
            HasReturnValue = false;
            _chunks = constructor.GetInstructions().Select(i => converter.Convert(i, this));
            Code = GetCode();
        }

        public IMnemonicsStream GetMethodEpilogue()
        {
            return MnemonicStreamFactory.Create(
                "; method epilogue",
                HasReturnValue ? "pop eax" : string.Empty, // save result to stack
                "mov esp, ebp", // revert stack state
                "pop ebp",
                CleanArgumentsFromStack()
                );
        }

        private IMnemonicsStream CleanArgumentsFromStack()
        {
            if (ParametersCount == 0)
            {
                return MnemonicStreamFactory.Empty;
            }

            return MnemonicStreamFactory.Create(
                ";clean parameters from stack", 
                // esp + 0 - return address
                "pop ebx", // store return address
                $"add esp, {ParametersCount}*4", // clear arguments
                "push ebx" // push return address to stack
                );
        }

        public int GetLocalVariableOffset(int localIndex)
        {
            var localsSize = 4; // ebp - 4 beginning of locals
            for (var i = 0; i < localIndex; i++)
            {
                localsSize += _localVariables[i].LocalType.GetTypeSize();
            } 
            return -localsSize; // ebp - localsSizeBefore
        }

        public int GetArgumentOffset(int argIndex)
        {
            // ebp + 0 - ebp
            // ebp + 4 - return address
            // ebp + 8 - last argument
            return 4 + (ParametersCount - argIndex)*4;
        }

        public bool HasReturnValue { get; }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"{Label}:",
                GetMethodPrologue(),
                _chunks.Select(x => x.Code));
        }

        private IMnemonicsStream GetMethodPrologue()
        {
            return MnemonicStreamFactory.Create(
                "; method prologue",
                "push ebp",
                "mov ebp, esp", // save stack pointer
                $"sub esp, {GetLocalsSize()}"  //reserve place for locals
                );
        }

        private int GetLocalsSize()
        {
            return _localVariables.Sum(x => x.LocalType.GetTypeSize());
        }

        public IMnemonicsStream Code { get; }
    }
}
