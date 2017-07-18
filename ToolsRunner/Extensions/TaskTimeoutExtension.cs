using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRunner.Extensions
{
    internal static class TaskTimeoutExtension
    {
        public static async Task TimeoutAfter(this Task task, int millisecondsTimeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
            {
                await task;
            }
            else
            {
                throw new TimeoutException();
            } 
        }
    }
}
