
namespace Core.Entities;

public class Book : BaseEntity
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string CoverImageUrl { get; set; }
    public required string Genre { get; set; }
    public required string Publisher { get; set; }
    public int QuantityInStock { get; set; }
}