namespace AC.Core.API.Options
{
    public class SwaggerOptions
    {
        public const string Swagger = "Swagger";
        public string Description { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string LicenseName { get; set; } = string.Empty;
        public string LicenseUri { get; set; } = string.Empty;
    }
}