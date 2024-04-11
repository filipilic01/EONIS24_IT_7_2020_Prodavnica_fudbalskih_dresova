using API.Dtos.Jerseys;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class JerseyProfile : Profile
    {
        public JerseyProfile()
        {
            CreateMap<Jersey, JerseyDto>()
                .ForMember(d => d.Admin, o => o.MapFrom(s => s.Admin.AdminUserName));

            CreateMap<JerseyCreationDto, Jersey>()
              .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.AdminId));

            CreateMap<JerseyUpdateDto, Jersey>();

        }

    }
}
