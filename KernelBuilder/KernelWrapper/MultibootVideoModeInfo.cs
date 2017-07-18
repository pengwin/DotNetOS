using IL2AsmTranspiler;
using IL2AsmTranspiler.Interfaces;
using IL2AsmTranspiler.Interfaces.CodeChunks;

namespace KernelBuilder.KernelWrapper
{
    internal class MultibootVideoModeInfo : ICodeChunk
    {
        internal enum MultibootVideoMode : uint
        {
            LinearGrahics = 0,
            EgaTextMode = 1
        }

        internal enum MultibootPixelDepth : uint
        {
            EGA = 16,
            VGA = 256,
            Text = 0
        }

        private MultibootVideoMode VideoMode { get; }

        private uint Width { get; }

        private uint Height { get; }

        private MultibootPixelDepth Depth { get; }

        private MultibootVideoModeInfo(MultibootVideoMode videoMode, uint width, uint height, MultibootPixelDepth depth)
        {
            VideoMode = videoMode;
            Width = width;
            Height = height;
            Depth = depth;
        }

        private IMnemonicsStream GetCode()
        {
            return MnemonicStreamFactory.Create(
                $"\t dd 0x{(uint)VideoMode:X} ; video mode {VideoMode}",
                $"\t dd {Width} ; width",
                $"\t dd {Height} ; height",
                $"\t dd {(uint)Depth} ;depth");
        }

        public IMnemonicsStream Code => GetCode();

        public static MultibootVideoModeInfo EgaText80x25
            => new MultibootVideoModeInfo(MultibootVideoMode.EgaTextMode, 80, 25, MultibootPixelDepth.Text);

        public static MultibootVideoModeInfo Ega640x480x16
            => new MultibootVideoModeInfo(MultibootVideoMode.LinearGrahics, 640, 480, MultibootPixelDepth.EGA);
    }
}