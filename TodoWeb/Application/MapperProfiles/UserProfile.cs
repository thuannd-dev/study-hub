using AutoMapper;
using TodoWeb.Application.Dtos.UserModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateViewModel, User>();


        }
    }
}
