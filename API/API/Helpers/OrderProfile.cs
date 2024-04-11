using API.Dtos.JerseySizes;
using API.Dtos.Orders;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.Customer, o => o.MapFrom(s => (s.Customer.CustomerUserName)));

            CreateMap<OrderCreationDto, Order>()
              .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<OrderUpdateDto, Order>();

        }
    }
}
