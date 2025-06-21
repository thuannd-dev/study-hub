using AutoMapper;
using TodoWeb.Application.Dtos.ExamSubmissionsModel;
using TodoWeb.Application.Services.ExamQuestions;
using TodoWeb.Application.Services.Exams;
using TodoWeb.Application.Services.ExamSubmissionDetails;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.ExamSubmissions
{
    public class ExamSubbmissionService : IExamSubbmissionService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExamSubmissionDetailsService _examSubmissionDetailsService;
        private readonly IExamQuestionService _examQuestionService;
        public ExamSubbmissionService(IApplicationDbContext context, IMapper mapper, IExamSubmissionDetailsService examSubmissionDetailsService, IExamQuestionService examQuestionService, IExamService examService)
        {
            _context = context;
            _mapper = mapper;
            _examSubmissionDetailsService = examSubmissionDetailsService;
            _examQuestionService = examQuestionService;
        }

        public int CreateStudentExamSubmission(StudentExamSubmissionCreateModel newStudentExamSubmission)
        {
            //kiểm tra xem examId có tồn tại không, đã bị xóa hay chưa
            var exam = _context.Exams.FirstOrDefault(x => x.Id == newStudentExamSubmission.ExamId && x.Status != Constants.Enums.Status.Deleted);
            if (exam == null)
            {
                return -1;
            }
            //kiểm tra xem courseStudentId có tồn tại không, đã bị xóa hay chưa
            var courseStudent = _context.CourseStudent.FirstOrDefault(x => x.Id == newStudentExamSubmission.CourseStudentId);
            if (courseStudent == null)
            {
                return -2;
            }
            //kiểm tra xem questionId có bị trùng hay không
            var questionIds = newStudentExamSubmission.Answer.Select(x => x.QuestionId);
            var questionIdsDistinct = questionIds.Distinct();
            if (questionIds.Count() != questionIdsDistinct.Count())
            {
                return -3;
            }
            //kiểm tra xem questionId có tồn tại không, đã bị xóa hay chưa
            var questions = _context.Questions.Where(question => questionIds.Contains(question.Id) && question.Status != Constants.Enums.Status.Deleted);
            if (questions.Count() != questionIds.Count())
            {
                return -4;
            }
            //kiểm tra xem ChosenAnswer có giá trị hợp lệ không
            var chosenAnswersIsValid = newStudentExamSubmission.Answer.Select(x => x.ChosenAnswer).All(chosen => chosen == null || Enum.IsDefined(typeof(Choice), chosen));
            if (!chosenAnswersIsValid)
            {
                return -5;
            }

            //xong bước validate h đến bước tạo 
            //tạo mới examSubmission chưa có finalScore
            var data = _mapper.Map<ExamSubmission>(newStudentExamSubmission);
            _context.ExamSubmissions.Add(data);
            _context.SaveChanges();
            //tạo mới examSubmissionDetails
            foreach (var answer in newStudentExamSubmission.Answer)
            {
                var errorNumber = _examSubmissionDetailsService.CreateStudentExamSubmissionDetails(data.Id, answer);
                if (errorNumber == -1)
                {
                    return -6; // không tìm thấy QuestionId
                }
            }
            
            //số câu đúng
            var numberOfCorrectAnswer = _context.ExamSubmissionDetails.Where(x => x.ExamSubmissionId == data.Id && x.IsCorrect == true).Count();
            //tổng số câu
            var totalQuestion = _examQuestionService.GetExamQuestions(null, data.ExamId, null).Count();
            //tính finalScore
            var finalScore = (decimal)numberOfCorrectAnswer / totalQuestion * 10;
            //cập nhật finalScore cho examSubmission
            data.FinalScore = (double)finalScore;
            _context.ExamSubmissions.Update(data);
            //cập nhật finalScore cho Grade
            var grade = _context.Grades.OrderBy(grade => grade.Id).LastOrDefault(grade => grade.CourseStudentId == data.CourseStudentId);
            if (grade != null)
            {
                grade.FinalScore = finalScore;
                _context.Grades.Update(grade);
            }
            _context.SaveChanges();
            return data.Id;
        }
    }
}
