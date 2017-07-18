using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using IL2AsmTranspiler.Implementations;
using IL2AsmTranspiler.Implementations.CodeChunks;
using IL2AsmTranspiler.Implementations.Factories;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;
using IL2AsmTranspiler.Interfaces.Factories;
using Intrinsic;
using Module = Autofac.Module;

namespace IL2AsmTranspiler
{
    public class ResolveCore : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Decompiler>().As<IDecompiler>();
            builder.RegisterType<InstructionConverter>().As<IInstructionConverter>();
            builder.RegisterType<IntrinsicResolver>().As<IIntrinsicResolver>();
            builder.RegisterType<CodeContextFactory>().As<ICodeContextFactory>();
            builder.RegisterType<DecompilerFactory>().As<IDecompilerFactory>();
            builder.RegisterType<InstructionConverterFactory>().As<IInstructionConverterFactory>();
        }
    }
}
