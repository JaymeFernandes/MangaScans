using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response.Public_Routes;

public class CategoryDtoResponse
{
    public string Value { get; set; }
}

public static class CategoryExtensions
{
    public static CategoryDtoResponse ToLibraryResponse(this Category category)
        => new CategoryDtoResponse
        {
            Value = category.Name,
        };

    public static List<CategoryDtoResponse> ToLibraryResponse(this ICollection<Category> category)
        => category.Select(x => x.ToLibraryResponse()).ToList();
}