using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response;

public class CategoryDtoResponse
{
    public string Name { get; set; }
    public DateTime Create { get; set; }
}

public static class CategoryExtensions
{
    public static CategoryDtoResponse ToLibraryResponse(this Category category)
        => new CategoryDtoResponse
        {
            Name = category.Name,
            Create = category.CreatedAt
        };
}