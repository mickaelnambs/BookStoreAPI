using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IPhotoService
{
    Task<string> SavePhotoAsync(IFormFile file);
    void DeletePhoto(string path);
}