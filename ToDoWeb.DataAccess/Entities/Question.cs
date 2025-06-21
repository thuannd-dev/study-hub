using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class Question : ICreate, IUpdate, IDelete
    {
        public int Id { get; set;}
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public Choice CorrectAnswer { get; set; }
        public Role CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public Role? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        public Status Status { get; set; }
        public Role? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }

        // Navigation properties
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        
    }
}
