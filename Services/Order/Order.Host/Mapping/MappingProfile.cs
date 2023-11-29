using AutoMapper;
using Order.Host.Data.Entities;
using Order.Host.Models.DTOs;
using Order.Host.Models.Requests;

namespace Order.Host.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddOrderRequest, OrderItem>();
        CreateMap<OrderItem, OrderItemDto>();
    }
}