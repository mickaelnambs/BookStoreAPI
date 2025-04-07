namespace Core.Entities.OrderAggregate;

public class BookItemOrdered
{
    public int BookId { get; set; }
    public required string BookTitle { get; set; }
    public required string CoverImageUrl { get; set; }
}
