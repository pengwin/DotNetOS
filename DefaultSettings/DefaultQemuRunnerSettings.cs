using ToolsRunner.Interfaces;

namespace DefaultSettings
{
    public class DefaultQemuRunnerSettings : IQemuRunnerSettings
    {
        public string QemuExecutablePath => @"C:\Coding\osdev\Qemu-windows-2.5.0\qemu-system-i386.exe";
        public string QemuVersion => "2.5.0";
        public string BiosPath => @"C:\Coding\osdev\Qemu-windows-2.5.0\Bios\";
        public int TimeoutS => 2;
        public int MemorySizeMegs => 16;
    }
}
