using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IUnitOfWork unit, IPaymentService paymentService) : BaseApiController
{
    [HttpGet("orders")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders([FromQuery] OrderSpecParams specParams)
    {
        var spec = new OrderSpecification(specParams);

        return await CreatePagedResult(unit.Repository<Order>(), spec, specParams.PageIndex,
            specParams.PageSize);
    }

    [HttpGet("orders/{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var spec = new OrderSpecification(id);

        var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return BadRequest("No order with that id");

        return order.ToDto();
    }

    [HttpPost("orders/refund/{id:int}")]
    public async Task<ActionResult<OrderDto>> RefundOrder(int id)
    {
        var spec = new OrderSpecification(id);

        var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return BadRequest("No order with that id");

        if (order.Status == OrderStatus.Pending)
            return BadRequest("Payment not received for this order");

        var result = await paymentService.RefundPayment(order.PaymentIntentId);

        if (result == "succeeded")
        {
            order.Status = OrderStatus.Refunded;

            await unit.Complete();

            return order.ToDto();
        }

        return BadRequest("Problem refunding order");
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods()
    {
        var deliveryMethods = await unit.Repository<DeliveryMethod>().ListAllAsync();

        return Ok(deliveryMethods.Select(dm => dm.ToDto()));
    }

    [HttpGet("delivery-methods/{id:int}")]
    public async Task<ActionResult<DeliveryMethodDto>> GetDeliveryMethod(int id)
    {
        var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(id);

        if (deliveryMethod == null) return NotFound();

        return Ok(deliveryMethod.ToDto());
    }

    [HttpPost("delivery-methods")]
    public async Task<ActionResult<DeliveryMethod>> CreateDeliveryMethod(DeliveryMethodDto deliveryMethodDto)
    {
        var deliveryMethod = deliveryMethodDto.ToEntity();

        unit.Repository<DeliveryMethod>().Add(deliveryMethod);

        var result = await unit.Complete();

        if (!result) return BadRequest("Problem creating delivery method");

        return CreatedAtAction(nameof(GetDeliveryMethod), new { id = deliveryMethod.Id }, deliveryMethod.ToDto());
    }

    [HttpPut("delivery-methods/{id:int}")]
    public async Task<ActionResult<DeliveryMethod>> UpdateDeliveryMethod(int id, DeliveryMethodDto deliveryMethodDto)
    {
        if (id != deliveryMethodDto.Id) return BadRequest("Delivery method ID mismatch");

        var existingDeliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(id);

        if (existingDeliveryMethod == null) return NotFound();

        existingDeliveryMethod.UpdateFromDto(deliveryMethodDto);

        unit.Repository<DeliveryMethod>().Update(existingDeliveryMethod);

        var result = await unit.Complete();

        if (!result) return BadRequest("Problem updating delivery method");

        return Ok(existingDeliveryMethod);
    }

    [HttpDelete("delivery-methods/{id:int}")]
    public async Task<IActionResult> DeleteDeliveryMethod(int id)
    {
        var method = await unit.Repository<DeliveryMethod>().GetByIdAsync(id);
        if (method == null) return NotFound();

        unit.Repository<DeliveryMethod>().Remove(method);
        await unit.Complete();

        return NoContent();
    }
}
