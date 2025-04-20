using AutoMapper;
using TodoWeb.Application.Dtos.ExamModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.MapperProfiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile() 
        {
            //Map từ Exam sang ExamViewModel
            CreateMap<Exam, ExamViewModel>()
                .ForMember(dest => dest.ExamId, config => config.MapFrom(src => src.Id));

            //Map từ ExamCreateModel sang Exam
            CreateMap<ExamCreateModel, Exam>();

            //Map từ ExamUpdateModel sang Exam
            CreateMap<ExamUpdateModel, Exam>()
               .ForMember(dest => dest.Id, config => config.MapFrom(src => src.ExamId));
        }
    }
}
