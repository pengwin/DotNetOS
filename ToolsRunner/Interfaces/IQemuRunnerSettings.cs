using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRunner.Interfaces
{
    public interface IQemuRunnerSettings
    {
        string QemuExecutablePath { get; }
        string QemuVersion { get; }
        string BiosPath { get; }
        int TimeoutS { get; }

        int MemorySizeMegs { get; }
    }
}
