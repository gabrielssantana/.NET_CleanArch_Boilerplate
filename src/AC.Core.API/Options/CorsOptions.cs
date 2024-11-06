namespace AC.Core.API.Options
{
    public class CorsOptions
    {
        public const string Cors = "Cors";
        public string[] Headers { get; set; } = Array.Empty<string>();
        public string[] Methods { get; set; } = Array.Empty<string>();
        public string[] Origins { get; set; } = Array.Empty<string>();
    }
}