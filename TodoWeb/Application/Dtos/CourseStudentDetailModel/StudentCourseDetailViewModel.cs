using TodoWeb.Application.Dtos.CourseModel;

namespace TodoWeb.Application.Dtos.CourseStudentDetailModel
{
    public class StudentCourseDetailViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public List<CourseViewModel> Courses { get; set; }
    }


}
