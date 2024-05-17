using API.Dtos.VelicinaDresa;
using API.Dtos.Porudzbina;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class PorudzbinaProfile: Profile
    {
        public PorudzbinaProfile()
        {
            CreateMap<Porudzbina, PorudzbinaDto>()
                .ForMember(d => d.Kupac, o => o.MapFrom(s => (s.Kupac.KupacKorisnickoIme)));

            CreateMap<PorudzbinaCreationDto, Porudzbina>()
              .ForMember(dest => dest.KupacId, opt => opt.MapFrom(src => src.KupacId));

            CreateMap<PorudzbinaUpdateDto, Porudzbina>();

            CreateMap<Porudzbina, PorudzbinaPaymentDto>();

        }
    }
}
