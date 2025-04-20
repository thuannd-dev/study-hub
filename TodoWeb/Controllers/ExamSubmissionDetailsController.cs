using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.Dtos.ExamSubmissionDetailsModel;
using TodoWeb.Application.Services.ExamSubmissionDetails;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamSubmissionDetailsController : Controller
    {
        private readonly IExamSubmissionDetailsService _examSubmissionDetailsService;
        public ExamSubmissionDetailsController(IExamSubmissionDetailsService examSubmissionDetailsService)
        {
            _examSubmissionDetailsService = examSubmissionDetailsService;
        }

        [HttpPost]
        public IActionResult CreateExamSubmissionDetails([FromBody] ExamSubmissionDetailsCreateModel newExamSubmissionDetails)
        {
            
            var result = _examSubmissionDetailsService.CreateExamSubmissionDetails(newExamSubmissionDetails);
            if (result == -1)
            {
                return NotFound("ExamSubmissionId not found.");
            }
            else if (result == -2)
            {
                return NotFound("QuestionId not found.");
            }
            else if (result == -3)
            {
                return BadRequest("Duplicate QuestionId.");
            }
            else if (result == -4)
            {
                return BadRequest("Invalid ChosenAnswer.");
            }
            return Ok(result);
        }
    }
}
