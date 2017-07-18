using System;
using System.Reflection;

namespace IL2AsmTranspiler.Extensions
{
    internal static class LabelExtractorExtension
    {
        public static string GetMethodLabel(this MethodInfo method)
        {
            var reflectedTypeName = method.ReflectedType?.GetTypeLabel();
            return $"{reflectedTypeName}.{method.Name}";
        }

        public static string GetConstructorLabel(this ConstructorInfo constructor)
        {
            var reflectedTypeName = constructor.ReflectedType?.GetTypeLabel();
            return $"{reflectedTypeName}.{constructor.Name}";
        }

        public static string GetFieldLabel(this FieldInfo field)
        {
            var reflectedTypeName = field.ReflectedType?.GetTypeLabel();
            return $"{reflectedTypeName}.{field.Name}";
        }

        public static string GetTypeLabel(this Type type)
        {
            return type.FullName.Replace("+", "__");
        }
    }
}
