using System.IO;
using System.Reflection;
using System.Text;

namespace ToolsRunner.Tests.Infastructure
{
    internal static class ResourceHelper
    {
        private const string ResourceName = "ToolsRunner.Tests.Resources.multiboot.asm";

        public static string GetMultibootKernelSources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var multibootStream = assembly.GetManifestResourceStream(ResourceName);
            if (multibootStream == null)
            {
                throw new FileNotFoundException($"Resource {ResourceName} is not found");
            }
            var multibootBytes = new byte[multibootStream.Length];
            multibootStream.Read(multibootBytes, 0, multibootBytes.Length);
            return Encoding.ASCII.GetString(multibootBytes);
        }


    }
}
