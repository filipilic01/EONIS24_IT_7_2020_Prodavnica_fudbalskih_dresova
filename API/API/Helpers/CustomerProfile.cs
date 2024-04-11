using API.Dtos.Admins;
using API.Dtos.Customers;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();

            CreateMap<CustomerCreationDto, Customer>();

            CreateMap<CustomerUpdateDto, Customer>();
        }
    }
}
