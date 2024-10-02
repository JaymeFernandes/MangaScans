using System.Net.Mime;
using System.Text.Json.Serialization;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Shered;

public class UrlImageDto
{
    public string Link { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Sequence { get; set; } = 0;
}

public static class ImagesDtos
{
    public static UrlImageDto ToLibraryResponse(this ImagesCover imageCover)
        => new UrlImageDto()
        {
            Link = imageCover.Link,
        };

    public static List<UrlImageDto> ToLibraryResponse(this ICollection<ImagesCover> images)
        => images.Select(x => x.ToLibraryResponse()).ToList();

    public static UrlImageDto ToLibraryResponse(this ImagesChapter image)
        => new UrlImageDto()
        {
            Link = image.Url,
            Sequence = image.Sequence
        };
    
    public static List<UrlImageDto> ToLibraryResponse(this ICollection<ImagesChapter> images)
        => images.Select(x => x.ToLibraryResponse()).ToList();
}