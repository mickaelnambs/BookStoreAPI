using API.DTOs;
using Core.Entities;

namespace API.Extensions;

public static class DeliveryMethodExtensions
{
    public static DeliveryMethodDto ToDto(this DeliveryMethod deliveryMethod)
    {
        return new DeliveryMethodDto
        {
            Id = deliveryMethod.Id,
            ShortName = deliveryMethod.ShortName,
            DeliveryTime = deliveryMethod.DeliveryTime,
            Description = deliveryMethod.Description,
            Price = deliveryMethod.Price
        };
    }

    public static DeliveryMethod ToEntity(this DeliveryMethodDto deliveryMethodDto)
    {
        return new DeliveryMethod
        {
            Id = deliveryMethodDto.Id,
            ShortName = deliveryMethodDto.ShortName,
            DeliveryTime = deliveryMethodDto.DeliveryTime,
            Description = deliveryMethodDto.Description,
            Price = deliveryMethodDto.Price
        };
    }

    public static void UpdateFromDto(this DeliveryMethod deliveryMethod, DeliveryMethodDto dto)
    {
        deliveryMethod.ShortName = dto.ShortName;
        deliveryMethod.DeliveryTime = dto.DeliveryTime;
        deliveryMethod.Description = dto.Description;
        deliveryMethod.Price = dto.Price;
    }
}
