using Microsoft.Extensions.Configuration;

namespace Tipper
{
    public static class ConfigurationExtensions
    {
        public static bool IsProduction(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("Environment")?.ToUpperInvariant() == "PRODUCTION";
        }
    }
}
