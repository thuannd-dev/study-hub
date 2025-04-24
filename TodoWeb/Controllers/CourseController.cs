using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Services.Courses;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : Controller
    {
        //một instance của courseservice
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;
        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IEnumerable<CourseViewModel> GetCourse(int id)
        {
            _logger.LogInformation($"Get Course with id: {id}");
            if(id == 10)
            {
                _logger.LogWarning($"Warning: {id}");
            }
            if(id <= 0)
            {
                _logger.LogError($"Error: Id can't less 0");
            }
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
        public async Task<int> Post(PostCourseViewModel course)
        {
            try
            {
                return await _courseService.Post(course);
            }
            catch (Exception ex)
            {
                //log
                throw new Exception(ex.Message);
            }
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
