using System;
using System.Threading.Tasks;
using Common;

namespace KernelBuilder.Interfaces
{
    public interface IKernelBuilder
    {
        Task<IKernel> BuildAsync(Type kernelType, Option<Type> runtime, bool useTextMode = true);

        Task<IKernel> BuildAsync(Type kernelType, bool useTextMode = true);
    }
}
