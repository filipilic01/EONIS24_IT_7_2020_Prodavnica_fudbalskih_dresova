using API.Dtos.Dres;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class DresProfile : Profile
    {
        public DresProfile()
        {
            CreateMap<Dres, DresDto>()
                .ForMember(d => d.Admin, o => o.MapFrom(s => s.Admin.AdminKorisnickoIme));

            CreateMap<DresCreationDto, Dres>()
              .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.AdminId));

            CreateMap<DresUpdateDto, Dres>();

        }

    }
}
