using Core.Entities;

namespace Core.Specifications;

public class WishlistEntrySpecification : BaseSpecification<Wishlist>
{
    public WishlistEntrySpecification(string buyerEmail, int bookId)
        : base(w => w.BuyerEmail == buyerEmail && w.BookId == bookId)
    {
    }
}