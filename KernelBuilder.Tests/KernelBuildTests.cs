using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Tests;
using Intrinsic;
using KernelBuilder.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;

namespace KernelBuilder.Tests
{
    [TestClass]
    public class KernelBuildTests : BaseToolsTest
    {
        [TestMethod]
        public async Task KernelMultibootAddressTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(typeof(SimpleKernelWriteTestKernel)))
                {
                    await qemu
                     .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded")
                    .Stop()
                    .RunAsync();

                }
            }
        }

        internal static class SimpleKernelWriteTestKernel
        {

            [Intrinsic("hlt", "jmp $-1")]
            private static void Halt()
            {
            }

            public static void Entry()
            {
                Halt();
            }
        }
    }
}
