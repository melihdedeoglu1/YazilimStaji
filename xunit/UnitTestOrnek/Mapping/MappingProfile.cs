using AutoMapper;
using UnitTestOrnek.DTOs;
using UnitTestOrnek.Models;

namespace UnitTestOrnek.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();           

        }

    }
}
