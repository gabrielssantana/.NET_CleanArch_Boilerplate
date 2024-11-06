using AC.Core.Application.Behaviors;
using AC.Core.Application.Interfaces;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace AC.Core.Application.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddFluentValidation();
            services.AddMediator();
            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            });
            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(cfg =>
            {
                foreach (var assembly in assemblies)
                {
                    var iMapInterface = typeof(IMap);
                    var iMapMethodName = nameof(IMap.Mapping);
                    var typesThatImplementIMapInterface = assembly
                                                            .GetExportedTypes()
                                                            .Where(t => t.GetInterfaces().Any((t) => t.IsInterface && t == iMapInterface))
                                                            .ToList();

                    foreach (var typeThatImplementIMapInterface in typesThatImplementIMapInterface)
                    {
                        var type = typeThatImplementIMapInterface;
                        if (typeThatImplementIMapInterface.ContainsGenericParameters)
                        {
                            type = typeThatImplementIMapInterface.MakeGenericType(typeof(object));
                        }
                        var instanceOfTypeThatImplementIMapInterface = Activator.CreateInstance(type);
                        var implementedIMapMethod = type.GetMethod(iMapMethodName) ?? throw new NotImplementedException(iMapMethodName);
                        implementedIMapMethod.Invoke(instanceOfTypeThatImplementIMapInterface, [cfg]);
                    }
                }
            });
            return services;
        }
    }
}