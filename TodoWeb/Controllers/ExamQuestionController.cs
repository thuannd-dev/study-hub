using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.ExamQuestionModel;
using TodoWeb.Application.Services.ExamQuestions;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamQuestionController : Controller
    {
        //inject IExamQuestionService 
        private readonly IExamQuestionService _examQuestionService;
        public ExamQuestionController(IExamQuestionService examQuestionService)
        {
            _examQuestionService = examQuestionService;
        }

        [HttpGet]
        public IActionResult GetAllExamQuestions()
        {
            var result = _examQuestionService.GetExamQuestions(null, null, null);
            return Ok(result);
        }

        [HttpGet("{examQuestionId}")]
        public IActionResult GetExamQuestionById(int examQuestionId)
        {
            var result = _examQuestionService.GetExamQuestions(examQuestionId, null, null);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("exam/{examId}")]
        public IActionResult GetExamQuestionsByExamId(int examId)
        {
            var result = _examQuestionService.GetExamQuestions(null, examId, null);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("question/{questionId}")]
        public IActionResult GetExamQuestionsByQuestionId(int questionId)
        {
            var result = _examQuestionService.GetExamQuestions(null, null, questionId);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateExamQuestion([FromBody] ExamQuestionCreateModel newExamQuestion)
        {
            var result = _examQuestionService.CreateExamQuestion(newExamQuestion);
            if (result == -1)
            {
                return BadRequest("Invalid examId.");
            }
            if (result == -2)
            {
                return BadRequest("Invalid questionId.");
            }
            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateExamQuestion([FromBody] ExamQuestionUpdateModel updateExamQuestion)
        {
            var result = _examQuestionService.UpdateExamQuestion(updateExamQuestion);
            if (result == -1)
            {
                return NotFound("ExamQuestion not found.");
            }
            if (result == -2)
            {
                return BadRequest("Invalid examId.");
            }
            if (result == -3)
            {
                return BadRequest("Invalid questionId.");
            }
            return Ok(result);
        }


        [HttpDelete("{examQuestionId}")]
        public IActionResult DeleteExamQuestion(int examQuestionId)
        {
            var result = _examQuestionService.DeleteExamQuestion(examQuestionId);
            if (result == -1)
            {
                return NotFound("ExamQuestion not found.");
            }
            return Ok(result);
        }
    }
}
