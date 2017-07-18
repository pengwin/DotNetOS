using System;
using System.Linq;
using System.Reflection;
using Autofac;
using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using KernelBuilder.Implementations;
using KernelBuilder.Interfaces;
using Module = Autofac.Module;

namespace KernelBuilder
{
    public partial class ResolveCore : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SourceBuilder>().As<ISourceBuilder>();
            builder.RegisterType<Implementations.KernelBuilder>().As<IKernelBuilder>();
        }
    }
}
