using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Dtos.ExamSubmissionsModel
{
    public class StudentExamSubmissionCreateModel
    {
        public int ExamId { get; set; }
        public int CourseStudentId { get; set; }
        public IEnumerable<StudentChosenDetailModel> Answer { get; set; }
    }
}
