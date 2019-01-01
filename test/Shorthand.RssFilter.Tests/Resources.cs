using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shorthand.RssFilter.Tests {
    public static class Resources {
        public static async Task<string> GetStringResource(string resourceName) {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream("Shorthand.RssFilter.Tests.Resources." + resourceName);
            using(var reader = new StreamReader(resourceStream, Encoding.UTF8))
                return await reader.ReadToEndAsync();
        }
    }
}