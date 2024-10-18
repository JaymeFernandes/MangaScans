using System.Text.Json.Serialization;

namespace MangaScans.Application.DTOs.Response.User;

public class UserActionResponse
{
    public bool Success { get; set; }
    
    public List<string> Errors { get; set; } = new List<string>();

    public UserActionResponse(bool success)
    {
        Success = success;
        Errors = new();
    }
    
    public void AddError(string error)
        => Errors.Add(error);
    
    public void AddErrors(IEnumerable<string> errors)
        => Errors.AddRange(errors);
    
}