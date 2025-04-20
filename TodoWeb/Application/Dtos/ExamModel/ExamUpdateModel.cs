using TodoWeb.Constants.Enums;

namespace TodoWeb.Application.Dtos.ExamModel
{
    public class ExamUpdateModel
    {
        public int ExamId { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
