using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.Dtos.ExamSubmissionsModel;
using TodoWeb.Application.Services.ExamSubmissions;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamSubbmissionController : Controller
    {
        private readonly IExamSubbmissionService _examSubbmissionService;
        public ExamSubbmissionController(IExamSubbmissionService examSubbmissionService)
        {
            _examSubbmissionService = examSubbmissionService;
        }

        [HttpPost]
        public IActionResult CreateExamSubbmission([FromBody] StudentExamSubmissionCreateModel newStudentExamSubmission)
        {
            var result = _examSubbmissionService.CreateStudentExamSubmission(newStudentExamSubmission);
            if (result == -1)
            {
                return NotFound("ExamId not found.");
            }
            else if (result == -2)
            {
                return NotFound("CourseStudentId not found.");
            }
            else if (result == -3)
            {
                return BadRequest("Duplicate QuestionId.");
            }
            else if (result == -4 || result == -6)
            {
                return NotFound("QuestionId not found.");
            }
            else if (result == -5)
            {
                return BadRequest("ChosenAnswer Not Valid.");
            }
            return Ok(result);
        }
    }
}
