using TodoWeb.Application.Dtos.StudentModel;

namespace TodoWeb.Application.Dtos.CourseStudentDetailModel
{
    public class CourseStudentDetailViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public DateTime StartDate { get; set; }

        public List<StudentViewModel> Students { get; set; }
    }
}
