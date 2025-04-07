using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IBookService
{
    Task<Book?> CreateBookAsync(Book book, IFormFile coverImage);
    Task<bool> UpdateBookAsync(int id, Book book, IFormFile? coverImage);
    Task<bool> DeleteBookAsync(int id);
}
