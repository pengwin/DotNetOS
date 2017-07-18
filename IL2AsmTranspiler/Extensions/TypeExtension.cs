using System;

namespace IL2AsmTranspiler.Extensions
{
    public static class TypeExtension
    {
        public static int GetTypeSize(this Type type)
        {
            if (type.IsPrimitive)
            {
                return 4;
            }

            if (type.IsPointer)
            {
                return 4;
            }

            if (type.IsValueType)
            {
                return System.Runtime.InteropServices.Marshal.SizeOf(type);
            }

            throw new ArgumentException($"Unsupported type {type.Name}");
        }
    }
}
