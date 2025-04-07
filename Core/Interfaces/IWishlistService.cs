using Core.Entities;

namespace Core.Interfaces;

public interface IWishlistService
{
    Task<bool> AddToWishlistAsync(string buyerEmail, int bookId);
    Task<bool> RemoveFromWishlistAsync(string buyerEmail, int bookId);
    Task<List<Book>> GetWishlistBooksAsync(string buyerEmail);
    Task<bool> IsBookInWishlistAsync(string buyerEmail, int bookId);
}