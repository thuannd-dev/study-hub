using TodoWeb.Constants.Enums;

namespace TodoWeb.Application.Dtos.QuestionModel
{
    public class QuestionPostModel
    {
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public Choice CorrectAnswer { get; set; }
    }
}
