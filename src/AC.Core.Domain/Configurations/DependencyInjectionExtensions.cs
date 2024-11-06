using Microsoft.Extensions.DependencyInjection;

namespace AC.Core.Domain.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services) {
            return services;
        }
    }
}