using Microsoft.Extensions.Configuration;

namespace PhiRedaction.Core.Services.Settings
{
    public static class SettingsManager
    {
        private static IConfiguration _configuration;

        public static IConfiguration Configuration =>
            _configuration ?? throw new InvalidOperationException("SettingsManager has not been initialized. Call Initialize() first.");

        public static void Initialize(string configFilePath = "appsettings.json")
        {
            _configuration ??= new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(configFilePath, optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
