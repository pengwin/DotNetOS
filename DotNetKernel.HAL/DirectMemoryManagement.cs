using System;
using Intrinsic;

namespace DotNetKernel.HAL
{
    public static unsafe class DirectMemoryManagement
    {
        [Intrinsic("pop eax",
                   "push eax")]
        public static void* ToPtr(uint address)
        {
            return (void*) new IntPtr(address);
        }

        [Intrinsic("pop eax",
                   "push eax")]
        public static uint ToAddress(void* ptr)
        {
            return 0;
        }

        [Intrinsic("pop eax",
                   "push eax")]
        public static void* ToPtr(object someObject)
        {
            return (void*)new IntPtr(0);
        }

        public static byte* ToBytePtr(uint address)
        {
            return (byte*)ToPtr(address);
        }

        public static int* ToIntPtr(uint address)
        {
            return (int*)ToPtr(address);
        }
    }
}
