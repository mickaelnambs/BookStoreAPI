using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions;

public static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            BuyerEmail = order.BuyerEmail,
            OrderDate = order.OrderDate,
            ShippingAddress = order.ShippingAddress,
            PaymentSummary = order.PaymentSummary,
            DeliveryMethod = order.DeliveryMethod.Description,
            ShippingPrice = order.DeliveryMethod.Price,
            OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
            Subtotal = order.Subtotal,
            Discount = order.Discount,
            Total = order.GetTotal(),
            Status = order.Status.ToString(),
            PaymentIntentId = order.PaymentIntentId
        };
    }

    public static OrderItemDto ToDto(this OrderItem orderItem)
    {
        return new OrderItemDto
        {
            BookId = orderItem.ItemOrdered.BookId,
            BookTitle = orderItem.ItemOrdered.BookTitle,
            CoverImageUrl = orderItem.ItemOrdered.CoverImageUrl,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity
        };
    }
}