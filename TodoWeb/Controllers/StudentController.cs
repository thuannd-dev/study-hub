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
        [HttpGet("/Studentss")]
        public IActionResult SearchStudents([FromQuery] string search)
        {
            var result = _studentService.SearchStudents(search);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public StudentCourseDetailViewModel GetStudentDetails (int id)
        {
            return _studentService.GetStudentDetails(id);
        }

        [HttpGet("/students")]
        public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
        {
            return _studentService.GetStudents(schoolId);
        }

        [HttpGet("/Students")]
        public IActionResult GetStudents(
            [FromQuery] string sortBy,
            [FromQuery] bool desc,
            [FromQuery] int pageSize,
            [FromQuery] int pageIndex)
        {
            var data = _studentService.GetStudents(sortBy, desc, pageSize, pageIndex);
            if (data.TotalPages == 0 || data.Students.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(data);
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
