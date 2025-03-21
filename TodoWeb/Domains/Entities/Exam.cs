using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class Exam : ICreate, IUpdate, IDelete
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Status Status { get; set; } = Status.NotStarted;
        public Role CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public Role? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Role? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        //Navigation properties
        public Course Course { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<ExamSubmission> ExamSubmissions { get; set; }
    }
}
