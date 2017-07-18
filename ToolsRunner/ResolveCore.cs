using Autofac;
using ToolsRunner.Implementations;
using ToolsRunner.Interfaces;

namespace ToolsRunner
{
    public class ResolveCore<T, U> : Module 
        where T : IQemuRunnerSettings
        where U : IFasmRunnerSettings
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandRunner>().As<ICommandRunner>();
            builder.RegisterType<QemuTestBuilder>().As<IQemuTestBuilder>();
            builder.RegisterType<FasmRunner>().As<IFasmRunner>();
            builder.RegisterType<T>().As<IQemuRunnerSettings>();
            builder.RegisterType<U>().As<IFasmRunnerSettings>();
        }
    }
}
