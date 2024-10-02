using MangaScans.Domain.Entities;

namespace MangaScans.Api.Services;

public class ImageService
{
    private readonly string _imageDirectory;

    public ImageService()
    {
        _imageDirectory = $"{Directory.GetCurrentDirectory()}/StaticFiles/Covers/";

        if (!Directory.Exists(_imageDirectory))
            Directory.CreateDirectory(_imageDirectory);
    }

    public ImageService(string imageDirectory)
    {
        _imageDirectory = $"{Directory.GetCurrentDirectory()}/StaticFiles/Covers/{Path.GetDirectoryName(imageDirectory)}";

        if (!Directory.Exists(_imageDirectory))
            Directory.CreateDirectory(_imageDirectory);
    }

    public async Task<ImagesCover> SaveCoverImageAsync(IFormFile file, string mangaName, string mangaId)
    {
        string extension = Path.GetExtension(file.FileName);
        
        if (file == null || file.Length == 0)
            throw new ArgumentNullException("File is required.");
        
        if (extension?.ToLower() != ".png" || file.ContentType != "image/png")
            throw new Exception("The file must be a PNG image.");

        string pathFile = $"{_imageDirectory}{mangaName}_{mangaId}.png";
        
        try
        {
            await using (var stream = new FileStream(pathFile, FileMode.Create))
                await file.CopyToAsync(stream);
            
            return new ImagesCover($"Images/Covers/{mangaName}_{mangaId}.png") { MangaId = mangaId };
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception);
            
            return null;
        }
    }
    
    

    
    
}