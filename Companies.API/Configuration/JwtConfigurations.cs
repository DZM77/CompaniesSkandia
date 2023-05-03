namespace Companies.API.Configuration
{
    public class JwtConfigurations
    {
        public const string Section = "JwtSettings";
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public int Expires { get; set; }
    }
}
