using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.ExamModel;
using TodoWeb.Application.Services.Exams;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        public IActionResult GetAllExams()
        {
            var exams = _examService.GetExams(null, null);
            return Ok(exams);
        }

        [HttpGet("{examId}")]
        public IActionResult GetExamById(int examId)
        {
            var exams = _examService.GetExams(examId, null);
            if (exams.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(exams);
        }

        [HttpGet("course/{courseId}")]
        public IActionResult GetExamsByCourseId(int courseId)
        {
            var exams = _examService.GetExams(null, courseId);
            if (exams.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(exams);
        }

        [HttpPost]
        public IActionResult CreateExam([FromBody] ExamCreateModel newExam)
        {
            var result = _examService.PostExam(newExam);
            if (result == -1)
            {
                return BadRequest("Invalid exam data.");
            }
            if (result == -2)
            {
                return NotFound("Course not found.");
            }
            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateExam([FromBody] ExamUpdateModel updateExam)
        {
            var result = _examService.PutExam(updateExam);
            if (result == -3)
            {
                return BadRequest("Invalid exam data.");
            }
            if (result == -2)
            {
                return NotFound("Course not found.");
            }
            if (result == -1)
            {
                return NotFound("Exam not found.");
            }
            return Ok(result);
        }

        [HttpDelete("{examId}")]
        public IActionResult DeleteExam(int examId)
        {
            var result = _examService.DeleteExam(examId);
            if (result == -1)
            {
                return NotFound("Exam not found.");
            }
            return Ok(result);
        }
    }
}
