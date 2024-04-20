using API.Dtos.Admin;

using API.Dtos.Kupac;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class KupacProfile : Profile
    {
        public KupacProfile()
        {
            CreateMap<Kupac, KupacDto>();

            CreateMap<KupacCreationDto, Kupac>();

            CreateMap<KupacUpdateDto, Kupac>();
        }
    }
}
