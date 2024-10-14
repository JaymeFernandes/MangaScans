using System.ComponentModel.DataAnnotations;

namespace MangaScans.Application.DTOs.Request.User;

public class RegisterDtoRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string Password { get; set; }
    
    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}