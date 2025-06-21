namespace TodoWeb.Domains.Entities
{
    public class ExamQuestion
    {
        //liên kết  nhiều nhiều giữa Exam và Question
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        //Navigation properties
        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
