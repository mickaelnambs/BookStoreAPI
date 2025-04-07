using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class WishlistService(IUnitOfWork unit) : IWishlistService
{
    public async Task<bool> AddToWishlistAsync(string buyerEmail, int bookId)
    {
        var book = await unit.Repository<Book>().GetByIdAsync(bookId);
        if (book == null) return false;

        var wishlistEntry = new Wishlist
        {
            BuyerEmail = buyerEmail,
            BookId = bookId,
            Book = book
        };

        unit.Repository<Wishlist>().Add(wishlistEntry);

        return await unit.Complete();
    }

    public async Task<bool> RemoveFromWishlistAsync(string buyerEmail, int bookId)
    {
        var wishlistEntry = await unit.Repository<Wishlist>()
            .GetEntityWithSpec(new WishlistEntrySpecification(buyerEmail, bookId));

        if (wishlistEntry == null) return false;

        unit.Repository<Wishlist>().Remove(wishlistEntry);

        return await unit.Complete();
    }

    public async Task<List<Book>> GetWishlistBooksAsync(string buyerEmail)
    {
        var spec = new WishlistEntriesWithBooksSpecification(buyerEmail);
        var wishlistEntries = await unit.Repository<Wishlist>().ListAsync(spec);

        return wishlistEntries.Select(entry => entry.Book).ToList();
    }

    public async Task<bool> IsBookInWishlistAsync(string buyerEmail, int bookId)
    {
        var entry = await unit.Repository<Wishlist>()
            .GetEntityWithSpec(new WishlistEntrySpecification(buyerEmail, bookId));

        return entry != null;
    }
}
