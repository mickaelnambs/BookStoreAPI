namespace Core.Entities;

public class CartItem
{
    public int BookId { get; set; }
    public required string BookTitle { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public required string CoverImageUrl { get; set; }
    public required string Genre { get; set; }
    public required string Publisher { get; set; }
}
