using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.QuestionModel;
using TodoWeb.Application.Services.Questions;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : Controller
    {
        //tạo 1 instance của question service
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            return Ok(_questionService.GetQuestions(null));
        }

        [HttpGet("{questionId}")]
        
        public IActionResult GetQuestion(int questionId)
        {
            var question = _questionService.GetQuestions(questionId);
            if (question.IsNullOrEmpty())
            {
                return NotFound("Question not found");
            }
            return Ok(question);
        }

        [HttpPost]
        public IActionResult PostQuestion([FromBody] QuestionPostModel newQuestion)
        {
            var result = _questionService.PostQuestion(newQuestion);
            if (result == -1)
            {
                return BadRequest("Invalid data");
            }
            return Ok(result);
        }

        [HttpPut]
        public IActionResult PutQuestion([FromBody] QuestionPutModel updateQuestion)
        {
            var result = _questionService.PutQuestion(updateQuestion);
            if (result == -1)
            {
                return BadRequest("Invalid data");
            }
            if (result == -2)
            {
                return NotFound("Question not found");
            }
            return Ok(result);
        }

        [HttpDelete("{questionId}")]
        public IActionResult DeleteQuestion(int questionId)
        {
            var result = _questionService.DeleteQuestion(questionId);
            if (result == -2)
            {
                return NotFound("Question not found");
            }
            return Ok(result);
        }


    }
}
