namespace Core.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public BookItemOrdered ItemOrdered { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
