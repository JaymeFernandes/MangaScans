using System.Text.Json.Serialization;
using MangaScans.Application.DTOs.Shered;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response.Public_Routes;

public class RecommendationDtoResponse
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    public int Num_chapters { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CategoryDtoResponse>? Categories { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UrlImageDto Cover { get; set; } 
}

public static class RecommendationDtoExtensions
{
    public static RecommendationDtoResponse RecommendationToLibraryResponse(this Manga manga) =>
        new RecommendationDtoResponse()
        {
            Id = manga.Id,
            Name = manga.Name,
            Num_chapters = (manga?.Chapters != null ? manga.Chapters.Count : 0),
            Categories = (manga?.Categories != null ? manga.Categories.ToLibraryResponse() : null),
            Cover = manga?.Cover != null ? 
                manga.Cover.ToLibraryResponse() : new UrlImageDto()
                    { Link = "Images/Default.png" }
        };

    public static List<RecommendationDtoResponse> RecommendationToLibraryResponse(this ICollection<Manga> mangas)
        => mangas.Select(x => x.RecommendationToLibraryResponse()).ToList();
}

