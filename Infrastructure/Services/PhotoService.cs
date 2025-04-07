using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class PhotoService(IWebHostEnvironment environment, ILogger<PhotoService> logger,
    IHttpContextAccessor httpContextAccessor) : IPhotoService
{
    private readonly string _webRootPath = Path.Combine(
        environment.ContentRootPath,
        "wwwroot"
    );

    public async Task<string> SavePhotoAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        if (!file.ContentType.StartsWith("image/"))
            throw new ArgumentException("File must be an image");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var uploadPath = Path.Combine(_webRootPath, "uploads", "books");

        Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/books/{fileName}";

            if (httpContextAccessor.HttpContext != null)
            {
                var request = httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                return $"{baseUrl}{relativePath}";
            }

            return relativePath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving photo");
            throw new Exception("Error saving photo", ex);
        }
    }

    public void DeletePhoto(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        var uri = new Uri(path);
        var relativePath = uri.AbsolutePath;

        var fullPath = Path.Combine(_webRootPath, relativePath.TrimStart('/'));

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting photo at {Path}", fullPath);
            }
        }
    }
}