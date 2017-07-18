using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsRunner.Interfaces;
using ToolsRunner.Tests.Infastructure;

namespace ToolsRunner.Tests.Tests
{
    [TestClass]
    public class FasmRunnerTests : BaseToolsTest
    {
        [TestMethod]
        public async Task SimpleCompileTest()
        {
            var fasm = Container.Resolve<IFasmRunner>();
             await fasm.CompileAsync("test.asm", "test.bin");
        }

        [TestMethod]
        public async Task CompileKernelTest()
        {
            var sourceFileName = "kernel.asm";
            var outputFileName = "kernel.bin";

            var sources = ResourceHelper.GetMultibootKernelSources();
            File.WriteAllText(sourceFileName, sources);

            var fasm = Container.Resolve<IFasmRunner>();
            var output = await fasm.CompileAsync(sourceFileName, outputFileName);

            Assert.IsTrue(File.Exists(sourceFileName));
            Assert.IsTrue(File.Exists(outputFileName));
            Assert.IsFalse(string.IsNullOrEmpty(output.Output));

            File.Delete(sourceFileName);
            File.Delete(outputFileName);
        }

        [TestMethod]
        public async Task CompileErrorKernelTest()
        {
            var sourceFileName = "kernel.asm";
            var outputFileName = "kernel.bin";

            var sources = @"use16
                            org 100h
                            mov ax,qb
                            jmp $";
            File.WriteAllText(sourceFileName, sources);

            var fasm = Container.Resolve<IFasmRunner>();
            var output = await fasm.CompileAsync(sourceFileName, outputFileName);

            Assert.IsFalse(string.IsNullOrEmpty(output.ErrorResult));

            File.Delete(sourceFileName);
        }
    }
}
