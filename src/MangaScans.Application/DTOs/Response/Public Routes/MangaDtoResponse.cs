using System.Text.Json.Serialization;
using MangaScans.Application.DTOs.Shered;
using MangaScans.Domain.Entities;

namespace MangaScans.Application.DTOs.Response.Public_Routes;

public class MangaDtoResponse
{
	public string Name { get; set; }
	
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string Description { get; set; }
	
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public UrlImageDto Cover { get; set; } 
	
	public int Views { get; set; }
	public int Likes { get; set; }
	public int Dislikes { get; set; }
	
	public DateTime Created { get; set; }
	
	
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public List<CategoryDtoResponse> Categories { get; set; }
	
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public ICollection<ChapterDtoResponse> Chapters { get; set; }
}

public static class MangaExtension
{
	public static MangaDtoResponse ToLibraryResponse(this Manga manga)
	{
		var Dto = new MangaDtoResponse()
		{
			Name = manga.Name,
			Description = manga.Description,
			Views = manga.Views,
			Likes = manga.Likes,
			Dislikes = manga.Dislikes,
			Created = manga.CreatedAt,
			Cover = manga?.Cover != null ? 
				manga.Cover.ToLibraryResponse() : new UrlImageDto()
					{ Link = "Images/Default.png" }
		};
		
		if (manga?.Chapters != null)
			Dto.Chapters = manga.Chapters.ToLibraryResponse();
		

		if (manga?.Categories != null) 
			Dto.Categories = manga.Categories.ToLibraryResponse();
		
		return Dto;
	}

	public static List<MangaDtoResponse> TolibraryResponse(this IEnumerable<Manga> mangas)
		=> mangas.Select(x => x.ToLibraryResponse()).ToList();
}