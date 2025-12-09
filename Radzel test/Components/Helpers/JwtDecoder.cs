using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class JwtDecoder
{
    public static UserInfo DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return new UserInfo
        {
            Email = jwt.Claims.FirstOrDefault(c => c.Type == "Email")?.Value,
            UserId = jwt.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
            Role = jwt.Claims.FirstOrDefault(c => c.Type == "Role")?.Value,
            FullName = jwt.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value,
            LastName = jwt.Claims.FirstOrDefault(c => c.Type == "lastName")?.Value,
            Expiration = jwt.ValidTo
        };
    }
}

public class UserInfo
{
    public string Email { get; set; }
    public string UserId { get; set; }
    public string Role { get; set; }
    public string FullName { get; set; }
    public string LastName { get; set; }
    public DateTime Expiration { get; set; }
}
