using TodoWeb.Application.Dtos.QuestionModel;

namespace TodoWeb.Application.Services.Questions
{
    public interface IQuestionService
    {
        
        public IEnumerable<QuestionViewModel> GetQuestions(int? questionId);
        public int PostQuestion(QuestionPostModel newQuestion);
        public int PutQuestion(QuestionPutModel updateQuestion);
        public int DeleteQuestion(int questionId);

    }
}
