using TodoWeb.Application.Dtos.CourseStudentModel;

namespace TodoWeb.Application.Services.CourseStudents
{
    public interface ICourseStudentService
    {
        public int PostCourseStudent(PostCourseStudentViewModel courseStudentViewModel);
    }
}
