using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.QuestionModel;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Questions
{
    public class QuestionService : IQuestionService
    {
        //inject and use AutoMapper
        //thêm thuộc tính IApplicationDbContext vào class, và khỏi tạo giá trị thông qua constructer để
        //từ đó class có phiên làm việc với cơ sở dữ liệu cho riêng mình
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        //constructer
        public QuestionService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public IEnumerable<QuestionViewModel> GetQuestions(int? questionId)
        {
            var questions = _context.Questions
                 .Where(question => question.Status != Status.Deleted);
            if (questionId.HasValue)
            {
                questions = questions.Where(question => question.Id == questionId);
            }

            return _mapper.ProjectTo<QuestionViewModel>(questions);
        }

        public int PostQuestion(QuestionPostModel newQuestion)
        {
            var isEmpty = String.IsNullOrEmpty(newQuestion.QuestionText) ||
                String.IsNullOrEmpty(newQuestion.OptionA) ||
                String.IsNullOrEmpty(newQuestion.OptionB) ||
                String.IsNullOrEmpty(newQuestion.OptionC) ||
                String.IsNullOrEmpty(newQuestion.OptionD);
            var correctAnswerIsValid = Enum.IsDefined(typeof(Choice), newQuestion.CorrectAnswer);
            if (isEmpty || !correctAnswerIsValid)
            {
                return -1;
            }
            var data = _mapper.Map<Question>(newQuestion);
            _context.Questions.Add(data);
            _context.SaveChanges();
            return data.Id;


        }

        public int PutQuestion(QuestionPutModel updateQuestion)
        {
            //kiểm tra field trong request
            var isEmpty = String.IsNullOrEmpty(updateQuestion.QuestionText) ||
                String.IsNullOrEmpty(updateQuestion.OptionA) ||
                String.IsNullOrEmpty(updateQuestion.OptionB) ||
                String.IsNullOrEmpty(updateQuestion.OptionC) ||
                String.IsNullOrEmpty(updateQuestion.OptionD);
            var correctAnswerIsValid = Enum.IsDefined(typeof(Choice), updateQuestion.CorrectAnswer);
            if (isEmpty || !correctAnswerIsValid)
            {
                return -1;
            }
            //tìm question
            var question = _context.Questions.Find(updateQuestion.QuestionId);
            if (question == null || question.Status == Status.Deleted)
            {
                return -2;
            }
            _mapper.Map(updateQuestion, question);
            _context.SaveChanges();
            return question.Id;

        }

        public int DeleteQuestion(int questionId)
        {
            //kiểm tra xem question có tồn tại hay không
            var question = _context.Questions.Find(questionId);
            if (question == null || question.Status == Status.Deleted)
            {
                return -2;
            }
            //đánh dấu là đã xóa
            _context.Questions.Remove(question);
            _context.SaveChanges();
            return question.Id;
        }
    }
}
