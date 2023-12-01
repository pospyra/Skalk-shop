using AutoMapper;
using Skalk.Common.DTO.User;
using Skalk.DAL.Entities;

namespace Skalk.BLL.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<NewUserDTO, User>();

            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
