using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;
using ToolsRunner.Tests.Infastructure;

namespace ToolsRunner.Tests.Tests
{
    [TestClass]
    public class QemuTestBuilderTests : BaseToolsTest
    {
        [TestMethod]
        public async Task SimpleTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                await qemu.Start("test.bin", FailAction).Stop().RunAsync();
            }
        }

        [TestMethod]
        public async Task WaitRegisterTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = new TestKernel(Container).Build())
                {
                    await qemu
                        .Start(kernel.KernelFileName, FailAction)
                        .WaitForRegisterValue("eax", 0x2BADB002)
                        .PrintDebugMesage("Multiboot kernel running")
                        .Stop()
                        .RunAsync();
                }
            }
        }

        [TestMethod]
        public async Task WaitAddressTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = new TestKernel(Container).Build())
                {
                    await qemu
                        .Start(kernel.KernelFileName, FailAction)
                        .WaitForAddressValue(0x00100000, 0x1BADB002)
                        .PrintDebugMesage("Multiboot kernel loaded")
                        .Stop()
                        .RunAsync();
                }
            }
        }
    }
}
