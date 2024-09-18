using System.Text.Json.Serialization;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response;

public class ChapterDtoResponse
{
    public int Id { get; set; }
    
    public int Num { get; set; } 
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<ImageDtoResponse> Images { get; set; }
}

public static class ChapterExtensions
{
    public static ChapterDtoResponse ToLibraryResponse(this Chapter chapter)
    {
        var dto = new ChapterDtoResponse()
        {
            Id = chapter.Id,
            Num = chapter.Num,
        };

        if (chapter._Images  != null)
        {
            dto.Images = new List<ImageDtoResponse>();
            
            foreach (var image in chapter._Images)
            {
                dto.Images.Add(image.ToLibraryResponse());
            }
        }

        return dto;
    }

    public static List<ChapterDtoResponse> ToLibraryResponse(this IEnumerable<Chapter> chapters)
        => chapters.Select(c => c.ToLibraryResponse()).ToList();
}