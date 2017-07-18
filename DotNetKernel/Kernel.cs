using DotNetKernel.HAL;

namespace DotNetKernel
{
    public static unsafe class Kernel
    {
        public static void Entry()
        {
            var src = DirectMemoryManagement.ToBytePtr(0xB8000);
            *src = 0x2;
            src++;
            *src = 0xB;

            AppFlowControl.Halt();
            AppFlowControl.InfiniteLoop();
        }
    }
}
