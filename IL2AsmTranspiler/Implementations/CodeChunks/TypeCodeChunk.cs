using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Common;
using IL2AsmTranspiler.Extensions;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using Intrinsic;

namespace IL2AsmTranspiler.Implementations.CodeChunks
{
    internal class TypeCodeChunk : ITypeCodeChunk
    {
        private const BindingFlags SearchFlags = 
                              BindingFlags.DeclaredOnly | BindingFlags.Static |
                              BindingFlags.NonPublic | BindingFlags.Public |
                              BindingFlags.FlattenHierarchy | BindingFlags.Instance;

        private readonly IDictionary<string, IMethodCodeChunk> _methods = new Dictionary<string, IMethodCodeChunk>();

        private readonly IDictionary<string, IStaticFieldCodeChunk> _staticFields = new Dictionary<string, IStaticFieldCodeChunk>();

        private readonly IDictionary<string, IFieldCodeChunk> _fields = new Dictionary<string, IFieldCodeChunk>();

        private readonly Type _internalType;

        private readonly IInstructionConverter _converter;

        private Option<IMethodCodeChunk> _defaultConstructor; 

        public TypeCodeChunk(Type type, IInstructionConverter converter)
        {
            Label = type.GetTypeLabel();
            _internalType = type;
            _converter = converter;
            _defaultConstructor = Option<IMethodCodeChunk>.None;
        }

        public Option<IMethodCodeChunk> GetMethod(MethodInfo method)
        {
            if (method.ReflectedType != _internalType)
            {
                return Option<IMethodCodeChunk>.None;
            }
            return GetMethod(method.Name);
        }

        private Option<IMethodCodeChunk> GetPrivateDefaultStaticConstructor(Type type)
        {
            var constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Static, null,
                CallingConventions.Any, new Type[] {}, null);

            if (constructor == null)
            {
                return Option<IMethodCodeChunk>.None;
            }
            return Option<IMethodCodeChunk>.New(new MethodCodeChunk(constructor, _converter));
        }

        public Option<IStaticFieldCodeChunk> GetStaticField(FieldInfo field)
        {
            if (field.ReflectedType != _internalType)
            {
                return Option<IStaticFieldCodeChunk>.None;
            }

            return GetStaticFieldInternal(field.Name);
        }

        public Option<IFieldCodeChunk> GetField(FieldInfo field)
        {
            if (field.ReflectedType != _internalType)
            {
                return Option<IFieldCodeChunk>.None;
            }

            return GetFieldInternal(field.Name);
        }

        public Option<IMethodCodeChunk> DefaultStaticConstructor => _defaultConstructor;

        public string Label { get; }

        public Option<IMethodCodeChunk> GetMethod(string name)
        {
            IMethodCodeChunk result;
            if (!_methods.TryGetValue(name, out result))
            {
                var method = GetMethodFromType(name);
                if (method.IsNone)
                {
                    return method;
                }
                _methods[name] = method.Value;
                return method;
            }
            return Option<IMethodCodeChunk>.New(result);
        }

        private Option<IStaticFieldCodeChunk> GetStaticFieldInternal(string name)
        {
            IStaticFieldCodeChunk result;
            if (!_staticFields.TryGetValue(name, out result))
            {

                var field =_internalType.GetField(name, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
                if (field == null)
                {
                    return Option<IStaticFieldCodeChunk>.None;
                }
                result = new StaticFieldCodeChunk(field.GetFieldLabel(), field.FieldType);
                _staticFields[field.Name] = result;

                if (_defaultConstructor.IsNone)
                {
                    _defaultConstructor = GetPrivateDefaultStaticConstructor(_internalType);
                }
            }
            return Option<IStaticFieldCodeChunk>.New(result);
        }

        private Option<IFieldCodeChunk> GetFieldInternal(string name)
        {
            IFieldCodeChunk result;
            if (!_fields.TryGetValue(name, out result))
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                var allFields = _internalType.GetFields(flags);
                var field = _internalType.GetField(name, flags);
                if (field == null)
                {
                    return Option<IFieldCodeChunk>.None;
                }

                var index = Array.FindIndex(allFields, x => x == field);
                result = new FieldCodeChunk(index*4);
                _fields[field.Name] = result;
            }
            return Option<IFieldCodeChunk>.New(result);
        }

        private Option<IMethodCodeChunk> GetMethodFromType(string methodName)
        {
            var method = _internalType.GetMethod(methodName, SearchFlags);

            if (method?.GetMethodBody() == null)
            {
                return Option<IMethodCodeChunk>.None;
            }

            var intrinsicAttribute = method.GetCustomAttribute<IntrinsicAttribute>();
            if (intrinsicAttribute != null)
            {
                return Option<IMethodCodeChunk>.None; // skip intrinsic methods
            }

            var methodChunk = (IMethodCodeChunk) new MethodCodeChunk(method, _converter);
            return Option<IMethodCodeChunk>.New(methodChunk);
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"{Label}:",
                ";static fields",
                _staticFields.Values.Select(x => x.Code), 
                ";constructors",
                !_defaultConstructor.IsNone ? _defaultConstructor.Value.Code : MnemonicStreamFactory.Empty,
                ";methods",
                _methods.Values.Select(x => x.Code)
                );
        }

        public IMnemonicsStream Code => GetCode();
    }
}
