using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace ToolsRunner.Interfaces
{
    public interface ICommandRunner : IDisposable
    {
        void Start(string command, string[] commandParams);
        Task SendCommandAsync(string command);
        Option<string> ReadOutputLine();
        IEnumerable<string> ReadAllOutputLines();
        Task WaitForEndAsync(int timeoutMs);
        void Stop();
        Task<string> ToErrorOutputAsync();
    }
}