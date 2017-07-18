using System;

namespace Intrinsic
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IntrinsicAttribute : Attribute
    {
        public string[] Mnemonics { get; }

        public IntrinsicAttribute(params string[] assemblyLines)
        {
            Mnemonics = assemblyLines;
        }
    }
}