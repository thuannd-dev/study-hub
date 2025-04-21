using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Application.Services.Students;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("/SearchStudent/{searchTerm}")]
        public IActionResult SearchStudents(string searchTerm)
        {
            var result = _studentService.SearchStudents(searchTerm);
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

        [HttpGet]
        public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
        {
            return _studentService.GetStudents(schoolId);
        }

        [HttpGet("SortBy/{sortBy}/Desc/{desc}/PageSize/{pageSize}/PageIndex/{pageIndex}")]
        public IActionResult GetStudents(
            string sortBy,
            bool desc,
            int pageSize,
            int pageIndex)
        {

            var data = _studentService.GetStudents(sortBy, desc, pageSize, pageIndex);
            if (data.IsNullOrEmpty())
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
