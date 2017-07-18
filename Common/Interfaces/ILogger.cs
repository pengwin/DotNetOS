using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ILogger
    {
        void Debug(string component, string message);

        void Error(string component, string message);
    }
}
