using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace ntbs_ui_tests.Helpers
{
    public class ConfigHelper
    {
        public static IConfigurationRoot GetConfigurationRoot()
        {
            var outputPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("testSettings.json", optional: true)
                .AddUserSecrets("62C1600C-4252-429E-980B-0153542954A7")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
