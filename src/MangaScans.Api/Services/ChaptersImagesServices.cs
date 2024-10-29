using MangaScans.Domain.Entities;

namespace MangaScans.Api.Services
{
    public class ChaptersImagesService
    {
        private readonly string _imageDirectory;
        private readonly string _relativeDirectory;
        private readonly int _chapter;
        
        public ChaptersImagesService(string imageDirectory, int chapter)
        {
            string path = Directory.GetCurrentDirectory();
            _imageDirectory = Path.Combine(path, "StaticFiles", "Chapters", imageDirectory);
            _relativeDirectory = Path.Combine("Images", "Chapters", imageDirectory);
            _chapter = chapter;

            if (!Directory.Exists(_imageDirectory))
                Directory.CreateDirectory(_imageDirectory);
        }

        public async Task<ImagesChapter?> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("File is required.");

            string extension = Path.GetExtension(file.FileName)?.ToLower();
            if (extension != ".png" || file.ContentType != "image/png")
                throw new Exception("The file must be a PNG image.");

            try
            {
                string uniqueFileName = $"{Guid.NewGuid()}.png";
                string fullImagePath = Path.Combine(_imageDirectory, uniqueFileName);
                string relativeImagePath = Path.Combine(_relativeDirectory, uniqueFileName);

                await using (var stream = new FileStream(fullImagePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                return new ImagesChapter(relativeImagePath, _chapter) { Sequence = 0 };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}