using System.Reflection;
using System.Text;

using AC.Core.API.Options;
using AC.Core.Application.Configurations;
using AC.Core.CrossCutting.Configurations;
using AC.Core.Domain.Configurations;
using AC.Core.Infrastructure.Data.Configurations;

using Asp.Versioning;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.SwaggerGen;
using AC.Core.API.Middlewares;

namespace AC.Core.API.Configurations
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCrossCutting(configuration);
            services.AddApplication();
            services.AddDomain();
            services.AddInfrastructureData(configuration);
            services.AddAPIOptions(configuration);
            services
            .AddControllers()
            .AddNewtonsoftJson();
            services.AddApiVersioning();
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddCors(configuration);
            services
            .AddAuthentication("Bearer")
            .AddJwtBearer(configuration);
            services.AddAuthorization();
            return services;
        }

        private static IMvcBuilder AddNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            return builder;
        }

        private static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            var authenticationOptions = configuration
                                        .GetRequiredSection(Options.AuthenticationOptions.Authentication)
                                        .Get<Options.AuthenticationOptions>() ?? throw new Exception(nameof(Options.AuthenticationOptions));
            var key = Encoding.ASCII.GetBytes(authenticationOptions.Key);
            builder.AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return builder;
        }

        private static IServiceCollection AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.EnableAnnotations();
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Para utilizar a API é preciso se autenticar.",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme {
                        Reference = new OpenApiReference{
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme,
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }

        private static void AddApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers
                // "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
                options.Policies
                .Sunset(0.9)
                .Effective(DateTimeOffset.Now.AddDays(60))
                .Link("policy.html")
                .Title("Versioning Policy")
                .Type("text/html");
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        private static void AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = configuration
                                .GetRequiredSection(CorsOptions.Cors)
                                .Get<CorsOptions>() ?? throw new Exception(nameof(Options.AuthenticationOptions));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    {
                        policy.WithHeaders(corsOptions.Headers);
                        policy.WithMethods(corsOptions.Methods);
                        policy.WithOrigins(corsOptions.Origins);
                    });
            });
        }

        private static IServiceCollection AddAPIOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Options.AuthenticationOptions>(configuration.GetRequiredSection(Options.AuthenticationOptions.Authentication));
            services.Configure<SwaggerOptions>(configuration.GetRequiredSection(SwaggerOptions.Swagger));
            services.Configure<CorsOptions>(configuration.GetRequiredSection(CorsOptions.Cors));
            return services;
        }

        private static WebApplication UseAPIMiddlewares(this WebApplication app, IConfiguration configuration)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }

        public static WebApplication UseAPI(this WebApplication app, IConfiguration configuration)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
                app.Map("/", () => Results.Redirect("/swagger"));
            }

            app.UseAPIMiddlewares(configuration);
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}