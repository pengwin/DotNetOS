using System;
using System.IO;
using KernelBuilder.Interfaces;

namespace KernelBuilder.Implementations
{
    internal class Kernel : IKernel
    {
        public string Sources { get; }
        public string SourceFile { get; }
        public string KernelBinaryFile { get; }
        public string Directory { get; }
        public void Clean()
        {
            if (System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.Delete(Directory, true);
            }
        }

        public Kernel(string sources)
        {
            Sources = sources;
            Directory = Path.Combine("tmp", Guid.NewGuid().ToString());
            SourceFile = Path.Combine(Directory, "kernel.asm");
            KernelBinaryFile = Path.Combine(Directory, "kernel.bin");
            System.IO.Directory.CreateDirectory(Directory);
            File.WriteAllText(SourceFile, Sources);
        }

        public void Dispose()
        {
            Clean();
        }
    }
}
