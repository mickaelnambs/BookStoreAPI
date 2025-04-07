using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class BookService(IPhotoService photoService, IUnitOfWork unit) : IBookService
{
    public async Task<Book?> CreateBookAsync(Book book, IFormFile coverImage)
    {
        if (coverImage != null)
        {
            book.CoverImageUrl = await photoService.SavePhotoAsync(coverImage);
        }

        unit.Repository<Book>().Add(book);

        if (await unit.Complete())
        {
            return book;
        }

        return null;
    }

    public async Task<bool> UpdateBookAsync(int id, Book updatedBook, IFormFile? coverImage)
    {
        var existingBook = await unit.Repository<Book>().GetByIdAsync(id);

        if (existingBook == null) return false;

        if (updatedBook.Id != id) return false;

        ReflectionHelpers.UpdateProperties(updatedBook, existingBook, ["CoverImageUrl"]);

        if (coverImage != null)
        {
            if (!string.IsNullOrEmpty(existingBook.CoverImageUrl))
            {
                photoService.DeletePhoto(existingBook.CoverImageUrl);
            }

            existingBook.CoverImageUrl = await photoService.SavePhotoAsync(coverImage);
        }

        unit.Repository<Book>().Update(existingBook);

        return await unit.Complete();
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await unit.Repository<Book>().GetByIdAsync(id);

        if (book == null) return false;

        if (!string.IsNullOrEmpty(book.CoverImageUrl))
        {
            photoService.DeletePhoto(book.CoverImageUrl);
        }

        unit.Repository<Book>().Remove(book);

        return await unit.Complete();
    }
}
