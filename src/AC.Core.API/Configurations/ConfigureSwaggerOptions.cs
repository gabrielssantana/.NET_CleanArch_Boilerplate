using AC.Core.API.Options;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Text;

namespace AC.Core.API.Configurations
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly SwaggerOptions _swaggerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="swaggerOptions"></param>
        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IOptions<SwaggerOptions> swaggerOptions
        )
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _swaggerOptions = swaggerOptions.Value ?? throw new ArgumentNullException(nameof(swaggerOptions));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, _swaggerOptions));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, SwaggerOptions swaggerOptions)
        {
            var text = new StringBuilder(swaggerOptions.Description);
            var info = new OpenApiInfo()
            {
                Title = swaggerOptions.Title,
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact() { Name = swaggerOptions.ContactName, Email = swaggerOptions.ContactEmail },
                License = new OpenApiLicense() { Name = swaggerOptions.LicenseName, Url = new Uri(swaggerOptions.LicenseUri) }
            };

            if (description.IsDeprecated)
            {
                text.Append(" Esta versão da API está obsoleta.");
            }

            if (description.SunsetPolicy is SunsetPolicy policy)
            {
                if (policy.Date is DateTimeOffset when)
                {
                    text.Append(" Esta API sairá do ar em ")
                        .Append(when.Date.ToShortDateString())
                        .Append('.');
                }

                if (policy.HasLinks)
                {
                    text.AppendLine();

                    for (var i = 0; i < policy.Links.Count; i++)
                    {
                        var link = policy.Links[i];

                        if (link.Type == "text/html")
                        {
                            text.AppendLine();

                            if (link.Title.HasValue)
                            {
                                text.Append(link.Title.Value).Append(": ");
                            }

                            text.Append(link.LinkTarget.OriginalString);
                        }
                    }
                }
            }

            info.Description = text.ToString();

            return info;
        }
    }
}