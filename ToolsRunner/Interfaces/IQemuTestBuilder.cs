using System;
using System.Threading.Tasks;
using Common;
using ToolsRunner.Implementations;

namespace ToolsRunner.Interfaces
{
    public interface IQemuTestBuilder : IDisposable
    {
        IQemuTestBuilder Start(string kernel, Option<Action<string>> failAction);
        IQemuTestBuilder Stop();
        IQemuTestBuilder WaitForAddressValue(UInt32 address, UInt32 value);
        IQemuTestBuilder WaitForRegisterValue(string register, UInt32 value);
        IQemuTestBuilder PrintDebugMesage(string message);
        Task RunAsync();
    }
}