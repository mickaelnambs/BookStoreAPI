using Core.Entities;

namespace Core.Specifications;

public class WishlistEntriesWithBooksSpecification : BaseSpecification<Wishlist>
{
    public WishlistEntriesWithBooksSpecification(string buyerEmail)
        : base(w => w.BuyerEmail == buyerEmail)
    {
        AddInclude(w => w.Book);
        AddOrderBy(w => w.CreatedAt);
    }
}
