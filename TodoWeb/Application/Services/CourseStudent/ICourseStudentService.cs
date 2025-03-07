using TodoWeb.Application.Dtos.CourseStudentModel;

namespace TodoWeb.Application.Services.CourseStudent
{
    public interface ICourseStudentService
    {
        public int PostCourseStudent(PostCourseStudentViewModel courseStudentViewModel);
    }
}
