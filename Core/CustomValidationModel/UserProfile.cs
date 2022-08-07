using AutoMapper;
using GLS_BlazorMVC_PoC.Models;

namespace GLS_BlazorMVC_PoC.Data
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}
