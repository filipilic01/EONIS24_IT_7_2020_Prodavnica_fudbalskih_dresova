using API.Dtos.Jerseys;
using API.Dtos.JerseySizes;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class JerseySizeProfile: Profile
    {
        public JerseySizeProfile()
        {
            CreateMap<JerseySize, JerseySizeDto>()
                .ForMember(d => d.Jersey, o => o.MapFrom(s => (s.Jersey.PlayerName + " " + s.Jersey.Team + " " + s.Jersey.Season)));

            CreateMap<JerseySizeCreationDto, JerseySize>()
              .ForMember(dest => dest.JerseyId, opt => opt.MapFrom(src => src.JerseyId));

            CreateMap<JerseySizeUpdateDto, JerseySize>();

        }
    }
}
