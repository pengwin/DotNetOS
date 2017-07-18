using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Autofac;
using IL2AsmTranspiler.Extensions;
using IL2AsmTranspiler.Implementations.CodeChunks;
using IL2AsmTranspiler.Implementations.CodeChunks.Instructions;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using Mono.Reflection;

namespace IL2AsmTranspiler.Implementations
{
    internal class InstructionConverter : IInstructionConverter
    {

        private readonly IIntrinsicResolver _intrinsicResolver;

        private readonly ICodeContext _codeContext;

        private readonly IDictionary<OpCode, Func<Instruction, IMethodCodeChunk, ICodeChunk>> _converters;

        public InstructionConverter(IIntrinsicResolver intrinsicResolver, ICodeContext codeContext)
        {
            _codeContext = codeContext;
            _intrinsicResolver = intrinsicResolver;
            _converters = new Dictionary<OpCode, Func<Instruction, IMethodCodeChunk, ICodeChunk>>
            {
                {OpCodes.Nop, (i, m) => new NopCodeChunk()},
                {OpCodes.Pop, (i, m) => new PopCodeChunk()},
                {OpCodes.Ret, (i, m) => new RetInstructionChunk(m)},
                {OpCodes.Ldc_I4, (i, m) => new LdcI4CodeChunk((Int32)i.Operand)},
                {OpCodes.Ldc_I4_S, (i, m) => new LdcI4CodeChunk(System.Convert.ToInt32(i.Operand))},
                {OpCodes.Ldc_I4_2, (i, m) => new LdcI4CodeChunk(2)},
                {OpCodes.Ldc_I4_8, (i, m) => new LdcI4CodeChunk(8)},
                {OpCodes.Ldc_I4_1, (i, m) => new LdcI4CodeChunk(1)},
                {OpCodes.Ldc_I4_0, (i, m) => new LdcI4CodeChunk(0)},
                {OpCodes.Ldc_I4_3, (i, m) => new LdcI4CodeChunk(3)},
                {OpCodes.Ldc_I4_4, (i, m) => new LdcI4CodeChunk(4)},
                {OpCodes.Add, (i, m) => new AddCodeChunk()},
                {OpCodes.Sub, (i, m) => new SubCodeChunk()},
                {OpCodes.Mul, (i, m) => new MulCodeChunk()},
                {OpCodes.Div, (i, m) => new DivCodeChunk()},
                {OpCodes.Stind_I1, (i, m) => new StindI1CodeChunk()},
                {OpCodes.Stind_I2, (i, m) => new StindI2CodeChunk()},
                {OpCodes.Stind_I4, (i, m) => new StindI4CodeChunk()},
                {OpCodes.Ldind_U1, (i, m) => new LdindU1CodeChunk()},
                {OpCodes.Ldind_I4, (i, m) => new LdindI4CodeChunk()},
                {OpCodes.Stloc_0, (i, m) => new StLocCodeChunk(0, m)},
                {OpCodes.Stloc_1, (i, m) => new StLocCodeChunk(1, m)},
                {OpCodes.Stloc_2, (i, m) => new StLocCodeChunk(2, m)},
                {OpCodes.Stloc_3, (i, m) => new StLocCodeChunk(3, m)},
                {OpCodes.Stloc_S, (i, m) => new StLocCodeChunk((LocalVariableInfo)i.Operand, m)},
                {OpCodes.Ldloc_0, (i, m) => new LdLocCodeChunk(0, m)},
                {OpCodes.Ldloc_1, (i, m) => new LdLocCodeChunk(1, m)},
                {OpCodes.Ldloc_2, (i, m) => new LdLocCodeChunk(2, m)},
                {OpCodes.Ldloc_3, (i, m) => new LdLocCodeChunk(3, m)},
                {OpCodes.Ldloc_S, (i, m) => new LdLocCodeChunk((LocalVariableInfo)i.Operand, m)},
                {OpCodes.Ldloca_S, (i, m) => new LdLocaCodeChunk((LocalVariableInfo)i.Operand, m)},
                {OpCodes.Call, CallConverter},
                {OpCodes.Ldarg_0, (i, m) => new LdArgCodeChunk(0, m)},
                {OpCodes.Ldarg_1, (i, m) => new LdArgCodeChunk(1, m)},
                {OpCodes.Ldarg_2, (i, m) => new LdArgCodeChunk(2, m)},
                {OpCodes.Starg_S, (i, m) => new StArgSCodeChunk((ParameterInfo)i.Operand, m)},
                {OpCodes.Clt, (i, m) => new CltCodeChunk(GetLocalLabelForInstruction(i), GetGlobalLabelForInstruction(i, m))},
                {OpCodes.Br_S, (i, m) => new BrSCodeChunk(GetGlobalLabelForInstruction((Instruction)i.Operand, m))},
                {OpCodes.Brtrue_S, (i, m) => new BrTrueSCodeChunk(GetGlobalLabelForInstruction((Instruction)i.Operand, m))},
                {OpCodes.Brfalse_S, (i, m) => new BrFalseSCodeChunk(GetGlobalLabelForInstruction((Instruction)i.Operand, m)) },
                {OpCodes.Ldstr, LdStringConverter },
                {OpCodes.Newobj, NewObjConverter },
                {OpCodes.Ldsfld, LoadStaticFieldConverter },
                {OpCodes.Ldfld, LoadFieldConverter },
                {OpCodes.Stsfld, StoreStaticFieldConverter },
                {OpCodes.Stfld, StoreFieldConverter },
                {OpCodes.Conv_I, (i, m) => new ConvICodeChunk() },
                {OpCodes.Ceq, (i,m) => new CeqCodeChunk(GetLocalLabelForInstruction(i), GetGlobalLabelForInstruction(i, m)) },
                {OpCodes.Initobj, InitObjConverter }
                //{OpCodes.Box, (i,m) => new CeqCodeChunk(GetLocalLabelForInstruction(i), GetGlobalLabelForInstruction(i, m)) },
            };
        }

        public ICodeChunk Convert(Instruction instruction, IMethodCodeChunk currentMethodChunk)
        {
            Func<Instruction, IMethodCodeChunk, ICodeChunk> converter;
            if (!_converters.TryGetValue(instruction.OpCode, out converter))
            {
                throw new ArgumentException($"Unknown opcode {instruction.OpCode}", nameof(instruction));
            }

            return new InstructionWrapperCodeChunk(GetLocalLabelForInstruction(instruction), converter(instruction, currentMethodChunk), instruction.ToString());
        }

        private ICodeChunk CallConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var method = instruction.Operand as MethodInfo;
            var intrinsicChunk = _intrinsicResolver.Resolve(method);
            if (!intrinsicChunk.IsNone)
            {
                return intrinsicChunk.Value;
            }
            var methodChunk = _codeContext.ResolveMethod(method);
            if (!methodChunk.IsNone)
            {
                return new CallInstructionChunk(methodChunk.Value);
            }
            throw new ArgumentException($"Unknown method {method}", nameof(instruction));
        }

        private ICodeChunk InitObjConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var type = instruction.Operand as Type;
            var size = type.GetTypeSize();
            if (_codeContext.RuntimeDefinition.IsNone)
            {
                throw new ArgumentException("Runtime is not defined");
            }
            var memset = _codeContext.ResolveMethod(_codeContext.RuntimeDefinition.Value.Memset);
            if (memset.IsNone)
            {
                throw new ArgumentException("Memset is not defined in runtime");
            }

            return new InitObjCodeChunk(size, memset.Value);
        }

        private ICodeChunk NewObjConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var method = instruction.Operand as MethodInfo;
            var methodChunk = _codeContext.ResolveMethod(method);
            if (!methodChunk.IsNone)
            {
                return new CallInstructionChunk(methodChunk.Value);
            }
            throw new ArgumentException($"Unknown method {method}", nameof(instruction));
        }

        private ICodeChunk LdStringConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var operand = (string)instruction.Operand;
            var internedString = _codeContext.StringIntern(operand);
            return new LdstrCodeChunk(internedString.Label);
        }

        private ICodeChunk LoadStaticFieldConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var operand = (FieldInfo)instruction.Operand;
            var field = _codeContext.ResolveStaticField(operand);
            if (!field.IsNone)
            {
                return new LdsFldCodeChunk(field.Value);
            }
            throw new ArgumentException($"Unknown field {field}", nameof(instruction));
        }

        private ICodeChunk LoadFieldConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var operand = (FieldInfo)instruction.Operand;
            var field = _codeContext.ResolveField(operand);
            if (!field.IsNone)
            {
                return new LdFldCodeChunk(field.Value);
            }
            throw new ArgumentException($"Unknown field {field}", nameof(instruction));
        }

        private ICodeChunk StoreStaticFieldConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var operand = (FieldInfo)instruction.Operand;
            var field = _codeContext.ResolveStaticField(operand);
            if (!field.IsNone)
            {
                return new StsFldCodeChunk(field.Value);
            }
            throw new ArgumentException($"Unknown field {field}", nameof(instruction));
        }

        private ICodeChunk StoreFieldConverter(Instruction instruction, IMethodCodeChunk currentMethod)
        {
            var operand = (FieldInfo)instruction.Operand;
            var field = _codeContext.ResolveField(operand);
            if (!field.IsNone)
            {
                return new StFldCodeChunk(field.Value);
            }
            throw new ArgumentException($"Unknown field {field}", nameof(instruction));
        }

        private string GetLocalLabelForInstruction(Instruction instruction)
        {
            return $".IL_{instruction.Offset}";
        }

        private string GetGlobalLabelForInstruction(Instruction instruction, IMethodCodeChunk method)
        {
            return $"{method.Label}{GetLocalLabelForInstruction(instruction)}";
        }
    }
}