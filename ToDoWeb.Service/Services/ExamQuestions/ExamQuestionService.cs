using AutoMapper;
using TodoWeb.Application.Dtos.ExamQuestionModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;
using TodoWeb.Migrations;

namespace TodoWeb.Application.Services.ExamQuestions
{
    public class ExamQuestionService : IExamQuestionService
    {
        //inject and use Auto Mapper
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ExamQuestionService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int CreateExamQuestion(ExamQuestionCreateModel newExamQuestion)
        {
            //Kiểm tra xem thử examID hay questionID có tồn tại trong database không, đã bị xóa chưa
            var exam = _context.Exams.FirstOrDefault(exam => exam.Id == newExamQuestion.ExamId && exam.Status != Constants.Enums.Status.Deleted);
            if (exam == null)
            {
                return -1; // examId not found
            }
            var question = _context.Questions.FirstOrDefault(question => question.Id == newExamQuestion.QuestionId && question.Status != Constants.Enums.Status.Deleted);
            if (question == null)
            {
                return -2; // questionId not found
            }
            var data = _mapper.Map<ExamQuestion>(newExamQuestion);
            _context.ExamQuestions.Add(data);
            _context.SaveChanges();
            return data.Id; // trả về examQuestionId
        }


        public IEnumerable<ExamQuestionViewModel> GetExamQuestions(int? ExamQuestionId, int? ExamId, int? QuestionId)
        {
            IQueryable<ExamQuestion> query = _context.ExamQuestions.Where(examQuestion => examQuestion.Exam.Status != Constants.Enums.Status.Deleted && examQuestion.Question.Status != Constants.Enums.Status.Deleted);
            if (ExamQuestionId.HasValue)
            {
               query = query.Where(examQuestion => examQuestion.Id == ExamQuestionId);
            }
            if (ExamId.HasValue)
            {
                query = query.Where(examQuestion => examQuestion.ExamId == ExamId);
            }
            if (QuestionId.HasValue)
            {
                query = query.Where(examQuestion => examQuestion.QuestionId == QuestionId);
            }
            return _mapper.ProjectTo<ExamQuestionViewModel>(query);
        }

        public int UpdateExamQuestion(ExamQuestionUpdateModel updateExamQuestion)
        {
            //get examQuestionId trong database với điều kiện exam và question chưa bị xóa
            var examQuestionDb = _context.ExamQuestions
                .FirstOrDefault(examQuestion => examQuestion.Id == updateExamQuestion.ExamQuestionId
                    && examQuestion.Exam.Status != Constants.Enums.Status.Deleted 
                    && examQuestion.Question.Status != Constants.Enums.Status.Deleted);
            if(examQuestionDb == null)
            {
                return -1; // examQuestionId not found
            }
            //Kiểm tra xem thử examID hay questionID có tồn tại trong database không, đã bị xóa chưa
            var exam = _context.Exams.FirstOrDefault(exam => exam.Id == updateExamQuestion.ExamId && exam.Status != Constants.Enums.Status.Deleted);
            if (exam == null)
            {
                return -2; // examId not found
            }
            var question = _context.Questions.FirstOrDefault(question => question.Id == updateExamQuestion.QuestionId && question.Status != Constants.Enums.Status.Deleted);
            if (question == null)
            {
                return -3; // questionId not found
            }
            _mapper.Map(updateExamQuestion, examQuestionDb);
            _context.SaveChanges();
            return examQuestionDb.Id;
        }

        public int DeleteExamQuestion(int examQuestionId)
        {
            //get examQuestionId trong database với điều kiện exam và question chưa bị xóa
            var examQuestionDb = _context.ExamQuestions
                .FirstOrDefault(examQuestion => examQuestion.Id == examQuestionId
                    && examQuestion.Exam.Status != Constants.Enums.Status.Deleted
                    && examQuestion.Question.Status != Constants.Enums.Status.Deleted);
            if (examQuestionDb == null)
            {
                return -1; // examQuestionId not found
            }
            _context.ExamQuestions.Remove(examQuestionDb);
            _context.SaveChanges();
            return examQuestionDb.Id;
        }
    }
}
