using TodoWeb.Constants.Enums;

namespace TodoWeb.Application.Dtos.ExamSubmissionsModel
{
    public class StudentChosenDetailModel
    {
        public int QuestionId { get; set; }
        public Choice? ChosenAnswer { get; set; }
    }
}
