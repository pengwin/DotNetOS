using Intrinsic;

namespace DotNetKernel.HAL
{
    public static unsafe class Runtime
    {
        [Intrinsic("mov eax, __heap_top", "push eax")]
        private static uint GetBaseAddress()
        {
            return 100;
        }

        private static uint _heapLastAddress = 0;

        public static void MemSet(byte* src, byte value, int sizeInBytes)
        {
            for (int i = 0; i < sizeInBytes; i++)
            {
                *src = value;
                src++;
            }
        }

        public static void* MemAlloc(int sizeInBytes)
        {
            if (_heapLastAddress == 0)
            {
                _heapLastAddress = GetBaseAddress();
            }
            var heapBound = DirectMemoryManagement.ToBytePtr(_heapLastAddress);
            MemSet(heapBound, 0x0, sizeInBytes);
            heapBound += sizeInBytes;
            _heapLastAddress = DirectMemoryManagement.ToAddress(heapBound);
            return heapBound;
        }
    }
}
