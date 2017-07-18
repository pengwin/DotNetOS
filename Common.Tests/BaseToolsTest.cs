using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using DefaultSettings;
using DotNetKernel.HAL;
using KernelBuilder.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;

namespace Common.Tests
{
    public abstract class BaseToolsTest 
    {
        protected IContainer Container { get; }

        protected readonly Option<Action<string>> FailAction = Option<Action<string>>.New(Assert.Fail);

        protected BaseToolsTest()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ToolsRunner.ResolveCore<DefaultQemuRunnerSettings, DefaultFasmRunnerSettings>>();
            builder.RegisterModule<Common.ResolveCore>();
            builder.RegisterModule<IL2AsmTranspiler.ResolveCore>();
            builder.RegisterModule<KernelBuilder.ResolveCore>();

            Container = builder.Build();
        }

        protected async Task AssertKernel(Type kernelType, Action<IQemuTestBuilder> qemuActions)
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(kernelType, Option<Type>.New(typeof(Runtime))))
                {
                    qemu
                    .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded");

                    qemuActions(qemu);

                    qemu
                    .WaitForAddressValue(0x7C00, 0xEFD)
                    .PrintDebugMesage("Kernel halted")
                    .Stop();

                    await qemu.RunAsync();
                }
            }
        }
    }
}
