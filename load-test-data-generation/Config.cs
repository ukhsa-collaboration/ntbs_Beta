using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace load_test_data_generation
{
    internal class Config
    {
        public Config()
        {
            GetConfigurationRoot().Bind(this);
        }

        public string ConnectionString { get; set; }
        public bool GenerateUsers { get; set; }
        public int? NumberOfNotificationsToGenerate { get; set; }

        private static IConfigurationRoot GetConfigurationRoot()
        {
            var outputPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("testSettings.json", optional: true)
                .AddUserSecrets("8256234F-BCDC-4B3C-9B2F-15450C83BC66")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
