using AutoMapper;
using TodoWeb.Domains.Entities;
using ToDoWeb.Service.Dtos.CourseModel;

namespace TodoWeb.Application.MapperProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            //Map từ course sang CourseViewModel
            CreateMap<Course, CourseViewModel>()
                .ForMember(dest => dest.CourseId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.CourseName, config => config.MapFrom(src => src.Name))
                .ForMember(dest => dest.StartDate, config => config.MapFrom(src => src.StartDate));

        }
    }
}
