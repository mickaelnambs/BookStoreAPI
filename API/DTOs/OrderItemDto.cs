namespace API.DTOs;

public class OrderItemDto
{
    public int BookId { get; set; }
    public required string BookTitle { get; set; }
    public required string CoverImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}