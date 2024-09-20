using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response;

public class ImageDtoResponse
{
    public int Id { get; set; }
    public string Url { get; set; }
}

public static class ImageExtensions
{
    public static ImageDtoResponse ToLibraryResponse(this Images images)
        => new ImageDtoResponse
        {
            Id = images.Id,
            Url = images.Url
        };

    public static List<ImageDtoResponse> ToLibraryResponse(this IEnumerable<Images> images)
        => images.Select(x => x.ToLibraryResponse()).ToList();
}