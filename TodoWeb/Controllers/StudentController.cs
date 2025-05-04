using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.ActionFilters;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
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

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
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

        [HttpGet("{studentId}")]
        public IActionResult GetStudent(int studentId)
        {
            var result = _studentService.GetStudent(studentId);
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
            return _studentService.GetStudentDetails(id);
        }

        [HttpPost]
        public int Post(StudentViewModel student)
        {
            return _studentService.Post(student);
        }
        [HttpPut]
        public int Put(StudentViewModel student)
        {
            return _studentService.Put(student);
        }

        [HttpDelete]
        public int Delete(int studentID)
        {
            return _studentService.Delete(studentID);
        }
    }
}
