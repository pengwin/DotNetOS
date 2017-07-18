using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ToolsRunner.Interfaces;

namespace ToolsRunner.Tests.Infastructure
{
    internal class TestKernel : IDisposable
    {
        private readonly IContainer _container;

        public string SourcesFileName => "test_kernel.asm";

        public string KernelFileName => "test_kernel.bin";

        public TestKernel(IContainer container)
        {
            _container = container;
        }

        public TestKernel Build()
        {
            var sources = ResourceHelper.GetMultibootKernelSources();
            File.WriteAllText(SourcesFileName, sources);

            var fasm = _container.Resolve<IFasmRunner>();
            fasm.CompileAsync(SourcesFileName, KernelFileName);
            return this;
        }

        public void Dispose()
        {
            if (File.Exists(SourcesFileName))
            {
                File.Delete(SourcesFileName);
            }

            if (File.Exists(KernelFileName))
            {
                File.Delete(KernelFileName);
            }
        }
    }
}
