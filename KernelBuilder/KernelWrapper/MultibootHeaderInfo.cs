using System;
using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class MultibootHeaderInfo : ICodeChunk
    {
        [Flags]
        internal enum HeaderFlags : uint
        {
            /// <summary>
            /// If bit 0 in the ‘flags’ word is set, then all boot modules loaded along with the operating system 
            /// must be aligned on page (4KB) boundaries. 
            /// </summary>
            PageAlign = 1 << 0,
            /// <summary>
            /// If bit 1 in the ‘flags’ word is set, then information on available memory via at least the ‘mem_*’ fields
            ///  of the Multiboot information structure must be included.
            /// </summary>
            MemoryInfo = 1 << 0,
            /// <summary>
            /// If bit 2 in the ‘flags’ word is set, information about the video mode table
            ///  must be available to the kernel.
            /// </summary>
            VideoMode = 1 << 2,
            /// <summary>
            /// If bit 16 in the ‘flags’ word is set, then the fields at offsets 12-28 in the Multiboot header are valid,
            ///  and the boot loader should use them instead of the fields in the actual executable header to calculate where to load the OS image.
            /// </summary>
            UseMultibootAddressFields = 1 << 16
        }

        public  uint HeaderMagic => 0x1BADB002;

        public HeaderFlags Flags { get; set; }

        public MultibootAddressFields AddressFields { get; set; }

        public MultibootVideoModeInfo VideoModeInfo { get; set; }

        private uint GetChecksum()
        {
            return (uint)-(HeaderMagic + (uint)Flags);
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"\t dd 0x{HeaderMagic:X} ;header magic",
                $"\t dd 0x{(uint) Flags:X} ;flags {Flags}",
                $"\t dd 0x{GetChecksum():X} ;checksum");
        }

        public IMnemonicsStream Code => GetCode();
    }
}
