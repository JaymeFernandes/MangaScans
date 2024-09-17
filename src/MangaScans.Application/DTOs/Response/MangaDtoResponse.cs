using System.Text.Json.Serialization;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response;

public class MangaDtoResponse
{
    public string Id { get; set; }
    
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    
    public string Name { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Description { get; set; }
    
    public DateTime Created { get; set; }
    
    public CategoryDtoResponse Category { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<ChapterDtoResponse> Chapters { get; set; }
}

public static class MangaExtension
{
    public static MangaDtoResponse ToLibraryResponse(this Manga manga)
    {
        var Dto = new MangaDtoResponse()
        {
            Id = manga.Id,
            Views = manga.Views,
            Likes = manga.Likes,
            Dislikes = manga.Dislikes,
            Name = manga.Name,
            Description = manga.Description,
            Created = manga.CreatedAt,
            Category  =  manga._Categories.ToLibraryResponse()
        };
        
        if (manga._Chapters != null)
        {
            Dto.Chapters = new List<ChapterDtoResponse>();
            
            foreach (var chapter in manga._Chapters)
            {
                Dto.Chapters.Add(chapter.ToLibraryResponse());
            }
        }

        if (manga._Categories != null) Dto.Category = manga._Categories.ToLibraryResponse();
        
        return Dto;
    }

    public static List<MangaDtoResponse> TolibraryResponse(this IEnumerable<Manga> mangas)
        => mangas.Select(x => x.ToLibraryResponse()).ToList();
}