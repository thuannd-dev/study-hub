using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using ToDoWeb.Service.Dtos.CourseModel;

namespace TodoWeb.Application.Services.Courses
{
    public interface ICourseService
    {
        public Task<IEnumerable<CourseViewModel>> GetCourses(int? courseId);

        //public IEnumerable<CourseStudentDetailViewModel> GetCoursesDetail(int? courseId);

        public Task<int> Post(PostCourseViewModel course);
        public Task<int> Delete(int courseId);
        Task<int> Put(CourseViewModel course);
    }
}
