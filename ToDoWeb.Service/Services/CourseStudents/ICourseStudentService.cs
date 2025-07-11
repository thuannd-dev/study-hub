using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.CourseStudentModel;

namespace TodoWeb.Application.Services.CourseStudents
{
    public interface ICourseStudentService
    {
        IEnumerable<CourseStudentDetailViewModel> GetCoursesDetail(int? courseId);
        StudentCourseDetailViewModel GetStudentDetails(int id);
        public int PostCourseStudent(PostCourseStudentViewModel courseStudentViewModel);
    }
}
