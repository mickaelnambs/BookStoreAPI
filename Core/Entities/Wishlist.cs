namespace Core.Entities;

public class Wishlist : BaseEntity
{
    public required string BuyerEmail { get; set; }
    public int BookId { get; set; }
    public required Book Book { get; set; }
}
