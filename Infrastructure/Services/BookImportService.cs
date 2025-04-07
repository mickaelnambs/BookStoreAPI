using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;

namespace Infrastructure.Services;

public class BookImportService(IUnitOfWork unit) : IBookImportService
{
    public async Task ImportBooksFromJsonAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Book import file not found", filePath);

        var booksData = await File.ReadAllTextAsync(filePath);

        var books = JsonSerializer.Deserialize<List<Book>>(booksData);

        if (books == null) return;

        foreach (var book in books)
        {
            var existingBook = await unit.Repository<Book>().GetByIdAsync(book.Id);

            if (existingBook == null)
            {
                unit.Repository<Book>().Add(book);
            }
            else
            {
                ReflectionHelpers.UpdateProperties(book, existingBook);
                unit.Repository<Book>().Update(existingBook);
            }
        }

        await unit.Complete();
    }
}
