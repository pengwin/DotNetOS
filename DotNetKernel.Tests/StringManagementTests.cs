using System.Threading.Tasks;
using Autofac;
using Common.Tests;
using DotNetKernel.HAL;
using KernelBuilder.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;

namespace DotNetKernel.Tests
{
    [TestClass]
    public class StringManagementTests : BaseToolsTest
    {
        #region PrintStrKernel Test

        internal static unsafe class PrintStrKernel
        {
            public static void Entry()
            {
                Runtime.MemSet(DirectMemoryManagement.ToBytePtr(0xB8000), 0, 80 * 25);
                PrintStr("Hello!");

                AppFlowControl.Halt();
                AppFlowControl.InfiniteLoop();
            }

            private static void PrintStr(string message)
            {
                byte* vidMemPointer = DirectMemoryManagement.ToBytePtr(0xB8000);
                byte* messagePtr = StringManagement.ToBytePtr(message);
                for (int i = 0; i < StringManagement.GetLength(message); i++)
                {
                    *vidMemPointer = *messagePtr;
                    messagePtr++;
                    vidMemPointer++;
                    *vidMemPointer = 0x0F;
                    vidMemPointer++;
                }
            }
        }

        [TestMethod]
        public async Task PrintStrTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(typeof(PrintStrKernel)))
                {
                    await qemu
                    .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded")
                    .WaitForAddressValue(0xB8000, 0x0F650F48) //He
                    .WaitForAddressValue(0xB8004, 0x0F6C0F6C) //ll
                    .WaitForAddressValue(0xB8008, 0x0F210F6F) //o!
                    .PrintDebugMesage("String printed")
                    .Stop()
                    .RunAsync();
                }
            }
        }

        #endregion PrintStrKernel Test

        #region StringLength Test

        internal static unsafe class StringLengthKernel
        {
            public static void Entry()
            {
                var someString = "123";
                var length = StringManagement.GetLength(someString);
                int* vidMemPointer = (int*)DirectMemoryManagement.ToPtr(0xB8000);
                *vidMemPointer = length;
            }
        }

        [TestMethod]
        public async Task StringLengthTest()
        {
            await AssertKernel(typeof(StringLengthKernel), q =>
               q.WaitForAddressValue(0xB8000, 0x00000003)
               .PrintDebugMesage("String size was loaded")
                );
        }

        #endregion StringLength Test
    }
}
