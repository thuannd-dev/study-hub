using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Services.Courses;
using TodoWeb.Application.Services.CourseStudents;
using ToDoWeb.Service.Dtos.CourseModel;

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
        private readonly ICourseStudentService _courseStudentService;
        private readonly ILogger<CourseController> _logger;
        public CourseController(
            ICourseService courseService, ICourseStudentService courseStudentService,
            ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
            _courseStudentService = courseStudentService;
        }

        [TypeFilter(typeof(CacheFilter), Arguments = [10])]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseAsync(int id)
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
            var courses = await _courseService.GetCourses(id);
            if (courses == null || !courses.Any())
            {
                _logger.LogInformation($"No course found with id: {id}");
                return NotFound($"No course found with id: {id}");
            }
            return Ok(courses.Single());
            /// Single() sẽ lấy ra một phần tử duy nhất trong một danh sách chỉ có một phần tử, nếu không có hoặc có nhiều hơn một phần tử sẽ ném ra ngoại lệ
            /// FirstOrDefault() sẽ trả về phần tử đầu tiên hoặc null nếu không có phần tử nào
        }


        [HttpGet]
        public async Task<IEnumerable<CourseViewModel>> GetAllCourseAsync()//int? courseId//có giá trị hoặc là null
        {
            return await _courseService.GetCourses(null);
        }
        
        [HttpGet("Detail/{id}")]
        public IEnumerable<CourseStudentDetailViewModel> GetCourseDetails(int id)
        {
            return _courseStudentService.GetCoursesDetail(id);
        }

        [HttpGet("Detail")]
        public IEnumerable<CourseStudentDetailViewModel> GetAllCourseDetails()
        {
            return _courseStudentService.GetCoursesDetail(null);
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
        public async Task<int> Put(CourseViewModel course)
        {
            return await _courseService.Put(course);
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _courseService.Delete(id);
        }
    }
}
