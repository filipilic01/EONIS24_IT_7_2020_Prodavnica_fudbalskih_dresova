using API.Dtos.StavkaPorudzbine;
using API.Dtos.Porudzbina;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class StavkaPorudzbineProfile: Profile
    {
        public StavkaPorudzbineProfile()
        {
            CreateMap<StavkaPorudzbine, StavkaPorudzbineDto>()
                .ForMember(d => d.Porudzbina, o => o.MapFrom(s => (s.PorudzbinaId)))
                .ForMember(d => d.VelicinaDresa, o => o.MapFrom(s => (s.VelicinaDresa.VelicinaDresaVrednost + " " + s.VelicinaDresa.DresId)));

            CreateMap<StavkaPorudzbineCreationDto, StavkaPorudzbine>()
              .ForMember(dest => dest.PorudzbinaId, opt => opt.MapFrom(src => src.PorudzbinaId))
              .ForMember(dest => dest.VelicinaDresaId, opt => opt.MapFrom(src => src.VelicinaDresaId));

            CreateMap<StavkaPorudzbineUpdateDto, StavkaPorudzbine>();

        }
    }
}
