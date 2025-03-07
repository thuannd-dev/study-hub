using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Services.Course;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : Controller
    {
        //một instance của courseservice
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{id}")]
        public IEnumerable<CourseViewModel> GetCourse(int id)
        {
            return _courseService.GetCourses(id);
        }


        [HttpGet]
        public IEnumerable<CourseViewModel> GetAllCourse()//int? courseId//có giá trị hoặc là null
        {
            return _courseService.GetCourses(null);
        }
        
        [HttpGet("Detail/{id}")]
        public IEnumerable<CourseStudentDetailViewModel> GetCourseDetails(int id)
        {
            return _courseService.GetCoursesDetail(id);
        }

        [HttpGet("Detail")]
        public IEnumerable<CourseStudentDetailViewModel> GetAllCourseDetails()
        {
            return _courseService.GetCoursesDetail(null);
        }


        [HttpPost]
        public int Post(PostCourseViewModel course)
        {
            return _courseService.Post(course);
        }

        [HttpPut]
        public int Put(CourseViewModel course)
        {
            return _courseService.Put(course);
        }

        [HttpDelete]
        public int Delete(int id)
        {
            return _courseService.Delete(id);
        }
    }
}
