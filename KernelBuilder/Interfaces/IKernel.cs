using System;

namespace KernelBuilder.Interfaces
{
    public interface IKernel : IDisposable
    {
        string Sources { get; }

        string SourceFile { get; }

        string KernelBinaryFile { get; }

        string Directory { get; }

        void Clean();
    }
}
