using WebApi.Abstractions;

namespace WebApi.Configurations;

public class JwtConfigurationEntry : IConfigurationEntry
{
    public static string Key => "JwtSettings";

    public string SecretKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}