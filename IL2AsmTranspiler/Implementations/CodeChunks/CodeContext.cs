using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using IL2AsmTranspiler.Interfaces.Factories;

namespace IL2AsmTranspiler.Implementations.CodeChunks
{
    internal class CodeContext : ICodeContext
    {
        private readonly IDecompiler _decompilerFactory;

        private readonly IDictionary<Type, ITypeCodeChunk> _typeCache;

        private readonly IDictionary<string, IInernedStringCodeChunk> _internedStrings;

        public CodeContext(IDecompilerFactory decompilerFactory, Option<IRuntimeDefinition> runtimeDefinition)
        {
            _decompilerFactory = decompilerFactory.GetDecompiler(this);
            _typeCache = new Dictionary<Type, ITypeCodeChunk>();
            _internedStrings = new Dictionary<string, IInernedStringCodeChunk>();
            RuntimeDefinition = runtimeDefinition;
        }

        public Option<IRuntimeDefinition> RuntimeDefinition { get; }

        public Option<IMethodCodeChunk> ResolveMethod(MethodInfo method)
        {
            if (method.ReflectedType == null)
            {
                return Option<IMethodCodeChunk>.None;
            }

            var type = ResolveType(method.ReflectedType);
            if (type.IsNone)
            {
                return Option<IMethodCodeChunk>.None;
            }

            return type.Value.GetMethod(method);
        }

        public Option<ITypeCodeChunk> ResolveType(Type type)
        {
            ITypeCodeChunk result;
            if (_typeCache.TryGetValue(type, out result))
            {
                return Option<ITypeCodeChunk>.New(result);
            }
            result = _decompilerFactory.GetTypeBody(type);
            _typeCache[type] = result;
            return Option<ITypeCodeChunk>.New(result);
        }

        public Option<IStaticFieldCodeChunk> ResolveStaticField(FieldInfo field)
        {
            if (field.ReflectedType == null)
            {
                return Option<IStaticFieldCodeChunk>.None;
            }

            var type = ResolveType(field.ReflectedType);
            if (type.IsNone)
            {
                return Option<IStaticFieldCodeChunk>.None;
            }

            return type.Value.GetStaticField(field);
        }

        public Option<IFieldCodeChunk> ResolveField(FieldInfo field)
        {
            if (field.ReflectedType == null)
            {
                return Option<IFieldCodeChunk>.None;
            }

            var type = ResolveType(field.ReflectedType);
            if (type.IsNone)
            {
                return Option<IFieldCodeChunk>.None;
            }

            return type.Value.GetField(field);
        }

        public IInernedStringCodeChunk StringIntern(string stringToIntern)
        {
            IInernedStringCodeChunk result;
            if (_internedStrings.TryGetValue(stringToIntern, out result))
            {
                return result;
            }

            var label = $"str_{_internedStrings.Values.Count}";
            result = new InernedStringCodeChunk(label, stringToIntern);
            _internedStrings[stringToIntern] = result;
            return result;
        }

        public string DefaultConstructorsLabel => "__defaultConstructors";

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                "Code_context:",
                _typeCache.Values.Select(x => x.Code),
                "__strings:",
                _internedStrings.Values.Select(x => x.Code),
                $"{DefaultConstructorsLabel}:",
                _typeCache.Values.Where(x => !x.DefaultStaticConstructor.IsNone).Select(x => $"call {x.DefaultStaticConstructor.Value.Label}"),
                "ret");
        }

        public IMnemonicsStream Code => GetCode();
    }
}
