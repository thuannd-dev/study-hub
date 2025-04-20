using AutoMapper;
using TodoWeb.Application.Dtos.ExamQuestionModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.MapperProfiles
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile() 
        {
            //Map từ ExamQuestion sang ExamQuestionViewModel
            CreateMap<ExamQuestion, ExamQuestionViewModel>()
                .ForMember(dest => dest.ExamQuestionId, config => config.MapFrom(src => src.Id));

            //Map từ ExamQuestionCreateModel sang ExamQuestion
            CreateMap<ExamQuestionCreateModel, ExamQuestion>();

            //Map từ ExamQuestionUpdateModel sang ExamQuestion
            CreateMap<ExamQuestionUpdateModel, ExamQuestion>()
                .ForMember(dest => dest.Id, config => config.MapFrom(src => src.ExamQuestionId));
        }
    }
}
