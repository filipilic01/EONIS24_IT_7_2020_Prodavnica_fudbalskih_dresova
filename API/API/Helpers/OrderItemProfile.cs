using API.Dtos.OrderItems;
using API.Dtos.Orders;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class OrderItemProfile: Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.Order, o => o.MapFrom(s => (s.OrderId)))
                .ForMember(d => d.JerseySize, o => o.MapFrom(s => (s.JerseySize.JerseySizeValue + " " + s.JerseySize.JerseyId)));

            CreateMap<OrderItemCreationDto, OrderItem>()
              .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
              .ForMember(dest => dest.JerseySizeId, opt => opt.MapFrom(src => src.JerseySizeId));

            CreateMap<OrderItemUpdateDto, OrderItem>();

        }
    }
}
