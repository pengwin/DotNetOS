using Autofac;
using Common.Implementations;
using Common.Interfaces;

namespace Common
{
    public class ResolveCore : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DebugLogger>().As<ILogger>();
        }
    }
}
