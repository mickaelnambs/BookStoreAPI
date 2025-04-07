using API.DTOs;
using Core.Entities;

namespace API.Extensions;

public static class BookMappingExtensions
{
    public static Book ToEntity(this BaseBookDto dto)
    {
        return new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Isbn = dto.Isbn,
            Description = dto.Description,
            Price = dto.Price,
            Genre = dto.Genre,
            Publisher = dto.Publisher,
            QuantityInStock = dto.QuantityInStock,
            CoverImageUrl = null!
        };
    }

    public static Book ToEntity(this CreateBookDto dto, string coverImageUrl = "")
    {
        var book = ((BaseBookDto)dto).ToEntity();
        book.CoverImageUrl = coverImageUrl;
        return book;
    }

    public static Book ToEntity(this UpdateBookDto dto)
    {
        var book = ((BaseBookDto)dto).ToEntity();
        book.Id = dto.Id;
        return book;
    }
}
