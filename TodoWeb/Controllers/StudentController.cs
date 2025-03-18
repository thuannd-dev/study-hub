using Microsoft.AspNetCore.Mvc;
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
