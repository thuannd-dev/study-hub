using AutoMapper;
using TodoWeb.Application.Dtos.ExamSubmissionDetailsModel;
using TodoWeb.Application.Dtos.ExamSubmissionsModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.MapperProfiles
{
    public class ExamSubmissionDetailsProfile : Profile
    {
        public ExamSubmissionDetailsProfile() 
        {
            CreateMap<ExamSubmissionDetailsCreateModel, ExamSubmissionDetail>();
            CreateMap<StudentChosenDetailModel, ExamSubmissionDetail>();


        }
    }
}
