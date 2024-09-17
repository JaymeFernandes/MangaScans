using System.ComponentModel.DataAnnotations;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Request;

public class MangaDtoRequest
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    [Range(1, 18,  ErrorMessage = "Value for {0} must be between {1} and {2}")]
    public int Category { get; set; }
}

public enum CategoryType
{
    Action = 1,
    Adventure = 2,
    Comedy = 3,
    Drama = 4,
    Romance = 5,
    Mystery = 6,
    Suspense = 7,
    Fantasy = 8,
    Sci_Fi = 9,
    Horror = 10,
    Slice_of_Life = 11,
    Supernatural = 12,
    Historical = 13,
    Sports = 14,
    Harem = 15,
    Yaoi = 16,
    Yuri = 17,
    Isekai = 18
}