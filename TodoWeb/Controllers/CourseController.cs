using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Services.Courses;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(LogFilter), Arguments = [LogLevel.Warning])]
    //typefilter tạo ra mỗi instance của LogFilter, mỗi lần gọi vào thì nó sẽ tạo ra một instance mới của LogFilter (giống với scope)
    //dùng khi muốn truyền tham số vào trong constructor của filter
    [AuditFilter]
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

        [TypeFilter(typeof(CacheFilter), Arguments = [10])]
        [HttpGet("{id}")]
        public IActionResult GetCourse(int id)
        {
            _logger.LogInformation($"Get Course with id: {id}");
            if(id == 10)
            {
                _logger.LogWarning($"Warning: {id}");
            }
            if(id <= 0)
            {
                _logger.LogError($"Error: Id can't less 0");
                throw new Exception("Id can't less 0");
            }
            return Ok(_courseService.GetCourses(id));
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
        public async Task<IActionResult> Post(PostCourseViewModel course)
        {
            try
            {
                return Ok(await _courseService.Post(course));
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
