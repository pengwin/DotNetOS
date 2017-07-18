namespace DotNetKernel.HAL
{
    public static unsafe class StringManagement
    {
        public static byte* ToBytePtr(string stringAddress)
        {
            var result = (byte*)DirectMemoryManagement.ToPtr(stringAddress);
            result += 4; // skip string length
            return result;
        }

        public static int GetLength(string stringAddress)
        {
            var ptr = (int*)DirectMemoryManagement.ToPtr(stringAddress);
            return *ptr;
        }
    }
}
