using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentModel;
using TodoWeb.Application.Services.CourseStudent;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseStudentController : Controller
    {
        private readonly ICourseStudentService _courseStudentService;

        public CourseStudentController(ICourseStudentService courseStudentService)
        {
            _courseStudentService = courseStudentService;
        }

        [HttpPost()]
        public int PostCourseStudent(PostCourseStudentViewModel courseStudentViewModel)
        {
            return _courseStudentService.PostCourseStudent(courseStudentViewModel);
        }
    }
}
