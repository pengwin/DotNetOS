using Intrinsic;

namespace DotNetKernel.HAL
{
    public static class AppFlowControl
    {
        [Intrinsic("jmp $")]
        public static void InfiniteLoop()
        {
            
        }

        
        [Intrinsic("hlt")]
        public static void Halt()
        {

        }
    }
}
