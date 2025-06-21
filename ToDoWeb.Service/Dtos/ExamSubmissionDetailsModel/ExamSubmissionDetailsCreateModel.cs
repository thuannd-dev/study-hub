using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Dtos.ExamSubmissionDetailsModel
{
    public class ExamSubmissionDetailsCreateModel
    {
        public int ExamSubmissionId { get; set; }
        public int QuestionId { get; set; }
        public Choice? ChosenAnswer { get; set; }
    }
}
