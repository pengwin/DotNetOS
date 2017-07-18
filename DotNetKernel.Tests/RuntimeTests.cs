using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class RuntimeTests : BaseToolsTest
    {
        #region NewObj Test

        private static class NewObjKernel
        {

            private class NewTestObj
            {
                public int Type { get; set; }
            }

            public static void Entry()
            {
                var test = new NewTestObj();
            }
        }

        [TestMethod]
        public async Task NewObjTest()
        {
            await AssertKernel(typeof(NewObjKernel), q =>
               q.WaitForAddressValue(0xB8000, 0x00000003)
               .PrintDebugMesage("String size was loaded")
                );
        }

        #endregion NewObj Test

        #region Malloc Test

        private static unsafe class MallocKernel
        {
            public static void Entry()
            {
                var testPointerOne = Runtime.MemAlloc(1024);
                var testPointerTwo = Runtime.MemAlloc(100);

                var addrOne = DirectMemoryManagement.ToAddress(testPointerOne);
                var addrTwo = DirectMemoryManagement.ToAddress(testPointerTwo);
                var addrDiff = addrTwo - addrOne;
                var vid = (uint*)DirectMemoryManagement.ToPtr(0xB8000);
                *vid = addrDiff;
            }

        }

        [TestMethod]
        public async Task MallocTest()
        {
            await AssertKernel(typeof(MallocKernel), q =>
               q.WaitForAddressValue(0xB8000, 100)
               .PrintDebugMesage("Memory was allocated")
                );
        }

        #endregion Malloc Test

        #region MemSetKernel

        internal static unsafe class MemSetKernel
        {
            public static void Entry()
            {
                byte* vidMemPointer = DirectMemoryManagement.ToBytePtr(0xB8000);
                Runtime.MemSet(vidMemPointer, 0, 80 * 25);

                AppFlowControl.Halt();
                AppFlowControl.InfiniteLoop();
            }
        }

        [TestMethod]
        public async Task MemSetTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(typeof(MemSetKernel)))
                {
                    await qemu
                    .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded")
                    .WaitForAddressValue(0xB8000, 0x00000000)
                    .PrintDebugMesage("Vid mem first address cleared")
                    .WaitForAddressValue(0xB8000 + 80 * 25 - 4, 0x00000000)
                    .PrintDebugMesage("Vid mem first address cleared")
                    .Stop()
                    .RunAsync();
                }
            }
        }

        #endregion MemSetKernel
    }
}
