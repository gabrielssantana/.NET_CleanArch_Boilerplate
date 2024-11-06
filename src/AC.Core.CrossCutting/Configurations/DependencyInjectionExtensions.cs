using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;

using Serilog;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace AC.Core.CrossCutting.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCrossCutting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSerilog(configuration);
            services.AddNewtonsoftJson();
            services.AddServices(configuration);
            return services;
        }
        private static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
            services.AddSerilog();
            return services;
        }
        private static IServiceCollection AddNewtonsoftJson(this IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}