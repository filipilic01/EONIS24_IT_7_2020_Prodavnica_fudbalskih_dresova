using API.Dtos.Dres;
using API.Dtos.VelicinaDresa;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class VelicinaDresaProfile: Profile
    {
        public VelicinaDresaProfile()
        {
            CreateMap<VelicinaDresa, VelicinaDresaDto>()
                .ForMember(d => d.Dres, o => o.MapFrom(s => s.DresId));

            CreateMap<VelicinaDresaCreationDto, VelicinaDresa>()
              .ForMember(dest => dest.DresId, opt => opt.MapFrom(src => src.DresId));

            CreateMap<VelicinaDresaUpdateDto, VelicinaDresa>();

        }
    }
}
