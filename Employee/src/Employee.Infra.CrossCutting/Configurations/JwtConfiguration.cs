namespace Employee.Infra.CrossCutting.Configurations;

public class JwtConfiguration
{
    public const string Section = "Jwt";
    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateIssuerSigningKey { get; set; } = true;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresInHrs { get; set; } = 2;
}