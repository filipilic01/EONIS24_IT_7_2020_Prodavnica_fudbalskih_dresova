using API.Dtos.Admin;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class AdminProfile: Profile
    {
        public AdminProfile() 
        {
            CreateMap<Admin, AdminDto>();

            CreateMap<AdminCreationDto, Admin>();

            CreateMap<AdminUpdateDto, Admin>();
        }
       
    }
}
