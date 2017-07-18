using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Implementations
{
    internal class DebugLogger : ILogger
    {
        public void Debug(string component, string message)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG: {message}", component);
        }

        public void Error(string component, string message)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR: {message}", component);
        }
    }
}
