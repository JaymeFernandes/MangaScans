using System.Text.Json.Serialization;
using MangaScans.Application.DTOs.Shered;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response.Public_Routes;

public class ChapterDtoResponse
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int Num_do_Capitulo { get; set; } 
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<UrlImageDto> Images { get; set; }
}

public static class ChapterExtensions
{
    public static ChapterDtoResponse ToLibraryResponse(this Chapter chapter)
    {
        
        var dto = new ChapterDtoResponse()
        {
            Id = chapter.Id,
            Title = chapter.Name,
            Num_do_Capitulo = chapter.Num,
        };

        if (chapter._Images  != null)
            dto.Images = chapter._Images.ToLibraryResponse();

        return dto;
    }

    public static List<ChapterDtoResponse> ToLibraryResponse(this IEnumerable<Chapter> chapters)
        => chapters.Select(c => c.ToLibraryResponse()).OrderByDescending(x => x.Num_do_Capitulo).Reverse().ToList();
}