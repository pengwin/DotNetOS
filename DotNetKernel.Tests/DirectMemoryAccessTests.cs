using System;
using System.Threading.Tasks;
using Autofac;
using Common.Tests;
using DefaultSettings;
using DotNetKernel.HAL;
using KernelBuilder.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;

namespace DotNetKernel.Tests
{
    [TestClass]
    public class DirectMemoryAccessTests : BaseToolsTest
    {
        #region ToPtr Test

        internal static unsafe class ToPtrKernel
        {
            public static void Entry()
            {
                var ptr = (byte*)DirectMemoryManagement.ToPtr(0xB8000);
                *ptr = 0x1;
                ptr++;
                *ptr = 0x1;
                ptr++;
                *ptr = 0x0;
                ptr++;
                *ptr = 0x0;
            }
        }

        [TestMethod]
        public async Task ToPtrTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(typeof(ToPtrKernel)))
                {
                    await qemu
                    .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded")
                    .WaitForAddressValue(0xB8000, 0x00000101)
                    .PrintDebugMesage("Vid mem address written")
                    .Stop()
                    .RunAsync(); 
                }
            }
        }

        #endregion ToPtr Test

        #region ToAddress Test

        internal static unsafe class ToAddressKernel
        {
            public static void Entry()
            {
                var vid = (uint*)DirectMemoryManagement.ToPtr(0xB8000);
                var ptr = (uint*)DirectMemoryManagement.ToPtr(0x7500);
                var address = DirectMemoryManagement.ToAddress(ptr);
                *vid = address;
            }
        }

        [TestMethod]
        public async Task ToAddressTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(typeof(ToAddressKernel)))
                {
                    await qemu
                    .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded")
                    .WaitForAddressValue(0xB8000, 0x7500)
                    .PrintDebugMesage("Address loaded")
                    .Stop()
                    .RunAsync();
                }
            }
        }

        #endregion ToAddress Test

        #region PtrOperations Test

        internal static unsafe class PtrOperationsKernel
        {
            public static void Entry()
            {
                var vid = (uint*)DirectMemoryManagement.ToPtr(0xB8000);
                var ptr = (uint*)DirectMemoryManagement.ToPtr(0x7500);
                ptr += 8;
                var address = DirectMemoryManagement.ToAddress(ptr);
                *vid = address;
            }
        }

        [TestMethod]
        public async Task PtrOperationsTest()
        {
            using (var qemu = Container.Resolve<IQemuTestBuilder>())
            {
                using (var kernel = await Container.Resolve<IKernelBuilder>().BuildAsync(typeof(PtrOperationsKernel)))
                {
                    await qemu
                    .Start(kernel.KernelBinaryFile, FailAction)
                    .WaitForAddressValue(0x00100000, 0x1BADB002)
                    .PrintDebugMesage("Multiboot kernel loaded")
                    .WaitForAddressValue(0xB8000, 0x7500 + 8 * 4)
                    .PrintDebugMesage("Address loaded")
                    .Stop()
                    .RunAsync();
                }
            }
        }

        #endregion PtrOperations Test
    }
}
