using System.Text.Json.Serialization;

namespace MangaScans.Application.DTOs.Response.User;

public class LoginDtoResponse
{
    public bool IsAuthenticated { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Address { get; set; }
    
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string RefreshToken { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime Expires { get; set; }
    
    public List<string> Erros { get; set; }
    
    public LoginDtoResponse() 
        => Erros = new List<string>();
    
    public LoginDtoResponse(bool isAuthenticated) : this() 
        => IsAuthenticated = isAuthenticated;

    public LoginDtoResponse(bool isAuthenticated, string token, string refreshToken) : this(isAuthenticated)
    {
        Token = token;
        RefreshToken = refreshToken;
        Expires = DateTime.UtcNow.AddHours(5);
    }
    
    public void AddErro(string error) 
        => Erros.Add(error);
    
    public void AddErros(IEnumerable<string> errors) 
        => Erros.AddRange(errors);
}