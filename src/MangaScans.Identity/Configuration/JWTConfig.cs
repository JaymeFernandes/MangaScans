using Microsoft.IdentityModel.Tokens;

namespace MangaScans.Identity.Configuration;

public class JWTConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public SigningCredentials SigningCredentials { get; set; }
}