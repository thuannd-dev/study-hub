using AutoMapper;
using TodoWeb.Application.Dtos.ExamSubmissionsModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.MapperProfiles
{
    public class ExamSubmissionProfile : Profile
    {
        public ExamSubmissionProfile()
        {
            CreateMap<StudentExamSubmissionCreateModel, ExamSubmission>();
        }
    
    }
}
