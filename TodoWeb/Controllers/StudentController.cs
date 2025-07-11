using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Application.Services.CourseStudents;
using TodoWeb.Application.Services.Students;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(LogFilter), Arguments = [LogLevel.Warning])]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ICourseStudentService _courseStudentService;

        public StudentController(IStudentService studentService, ICourseStudentService courseStudentService)
        {
            _studentService = studentService;
            _courseStudentService = courseStudentService;
        }
        [HttpGet("/search")]
        public IActionResult SearchStudents([FromQuery] string search_query)
        {
            var result = _studentService.SearchStudents(search_query);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("/AllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await _studentService.GetAllStudents();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudent(int studentId)
        {
            var result = await _studentService.GetStudents(studentId);
            return Ok(result);
        }

        [HttpGet("/Students")]
        public IActionResult GetStudents(
            [FromQuery] int? schoolId,
            [FromQuery] string? sortBy,
            [FromQuery] bool desc,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageIndex)
            
        {
            var result = _studentService.GetStudents(schoolId, sortBy, desc, pageSize, pageIndex);
            if (result.TotalPages == 0 || result.Students.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);

        }

        [HttpGet("/StudentDetails/{id}")]
        public StudentCourseDetailViewModel GetStudentDetails(int id)
        {
            return _courseStudentService.GetStudentDetails(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(StudentViewModel student)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetStudent), new { studentId = student.Id }, await _studentService.Post(student));
        }
        [HttpPut]
        public async Task<IActionResult> Put(StudentViewModel student)
        {
            await _studentService.Put(student);
            return NoContent();
        }

        [HttpDelete]
        public async Task<int> Delete(int studentID)
        {
            return await _studentService.Delete(studentID);
        }
    }
}
