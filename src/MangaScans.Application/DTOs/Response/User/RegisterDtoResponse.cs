namespace MangaScans.Application.DTOs.Response.User;

public class RegisterDtoResponse
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }

    
    public RegisterDtoResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Errors = new();
    }
    
    public void AddError(string error) 
        => Errors.Add(error);
    
    public void AddErrors(IEnumerable<string> errors)
        => Errors.AddRange(errors);
}