using AC.Core.Application.Interfaces.Data;
using AC.Core.Domain.Interfaces.Queries;
using AC.Core.Domain.Interfaces.Queries.GetUsers;
using AC.Core.Domain.Interfaces.Repositories;
using AC.Core.Infrastructure.Data.Contexts;
using AC.Core.Infrastructure.Data.Queries;
using AC.Core.Infrastructure.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AC.Core.Infrastructure.Data.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CoreDbContext>(
                builder =>
                {
                    builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
            );
            services.RunMigrations(configuration);
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CoreDbContext>());
            services.AddRepositoriesFromAssemblies();
            services.AddQueriesFromAssemblies();
            return services;
        }
        private static IServiceCollection RunMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            var runMigrations = configuration.GetValue<bool>("RunMigrations");
            using var serviceProvider = services.BuildServiceProvider();
            using var coreDbContext = serviceProvider.GetRequiredService<CoreDbContext>();
            if (runMigrations)
            {
                coreDbContext.Database.Migrate();
            }
            return services;
        }
        private static IServiceCollection AddRepositoriesFromAssemblies(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var repositoryTypes = assembly
                                        .GetTypes()
                                        .Where(t => t.IsClass && !t.IsAbstract &&
                                        t.IsGenericType == false &&
                                        t.BaseType != null &&
                                        t.BaseType.IsGenericType &&
                                        t.BaseType.GetGenericTypeDefinition() == typeof(RepositoryBase<>))
                                        .ToList();
                foreach (var type in repositoryTypes)
                {
                    var repoAssignableType = typeof(IRepository<>).MakeGenericType(type!.BaseType!.GetGenericArguments()[0]);
                    var repoInterface = type
                                        .GetInterfaces()
                                        .First(i => !i.IsGenericType && i.IsAssignableTo(repoAssignableType));
                    services.AddScoped(repoInterface, type);
                }
            }
            return services;
        }
        private static IServiceCollection AddQueriesFromAssemblies(this IServiceCollection services)
        {
            services.AddScoped<IGetUsersQuery, GetUsersQuery>();
            return services;
        }
    }
}