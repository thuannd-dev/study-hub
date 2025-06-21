using AutoMapper;
using TodoWeb.Application.Dtos.ExamSubmissionDetailsModel;
using TodoWeb.Application.Dtos.ExamSubmissionsModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.ExamSubmissionDetails
{
    public class ExamSubmissionDetailsService : IExamSubmissionDetailsService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExamSubmissionDetailsService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int CreateExamSubmissionDetails(ExamSubmissionDetailsCreateModel newExamSubmissionDetails)
        {
            //kiểm tra xem ExamSubmissionId có tồn tại trong bảng ExamSubmission không
            var examSubmission = _context.ExamSubmissions.Find(newExamSubmissionDetails.ExamSubmissionId);
            if (examSubmission == null)
            {
                return -1; // không tìm thấy ExamSubmissionId
            }

            //get question từ QuestionID với đk là Status != Deleted
            var question = _context.Questions.FirstOrDefault(q => q.Id == newExamSubmissionDetails.QuestionId && q.Status != Constants.Enums.Status.Deleted);
            if (question == null)
            {
                return -2; // không tìm thấy QuestionId
            }

            //kiểm tra xem với ExamSubmissionId và QuestionId này đã tồn tại trong bảng ExamSubmissionDetails chưa
            var existingExamSubmissionDetails = _context.ExamSubmissionDetails
                .FirstOrDefault(d => d.ExamSubmissionId == newExamSubmissionDetails.ExamSubmissionId && d.QuestionId == newExamSubmissionDetails.QuestionId);
            if (existingExamSubmissionDetails != null)
            {
                return -3; // đã tồn tại ExamSubmissionDetails với ExamSubmissionId và QuestionId này
            }

            //kiểm tra ChosenAnswer có hợp lệ không
            if (newExamSubmissionDetails.ChosenAnswer != null)
            {
                if (!Enum.IsDefined(typeof(Constants.Enums.Choice), newExamSubmissionDetails.ChosenAnswer))
                {
                    return -4; // ChosenAnswer không hợp lệ
                }
            }


            var examSubmissionDetails = _mapper.Map<ExamSubmissionDetail>(newExamSubmissionDetails);
            examSubmissionDetails.IsCorrect = question.CorrectAnswer == newExamSubmissionDetails.ChosenAnswer;
            _context.ExamSubmissionDetails.Add(examSubmissionDetails);
            _context.SaveChanges();
            return examSubmissionDetails.Id; // trả về Id của ExamSubmissionDetails vừa tạo
        }
    

        public int CreateStudentExamSubmissionDetails(int examSubmissionId, StudentChosenDetailModel studentChosenDetail)
        {
            //kiểm tra xem ExamSubmissionId có tồn tại trong bảng ExamSubmission không
            //var examSubmission = _context.ExamSubmissions.Find(examSubmissionId);
            //if (examSubmission == null)
            //{
            //    return -1; // không tìm thấy ExamSubmissionId
            //}

            //get question từ QuestionID với đk là Status != Deleted
            var question = _context.Questions.FirstOrDefault(q => q.Id == studentChosenDetail.QuestionId && q.Status != Constants.Enums.Status.Deleted);
            if (question == null)
            {
                return -1; // không tìm thấy QuestionId
            }

            //kiểm tra xem với ExamSubmissionId và QuestionId này đã tồn tại trong bảng ExamSubmissionDetails chưa
            //var existingExamSubmissionDetails = _context.ExamSubmissionDetails
            //    .FirstOrDefault(d => d.ExamSubmissionId == examSubmissionId && d.QuestionId == studentChosenDetail.QuestionId);
            //if (existingExamSubmissionDetails != null)
            //{
            //    return -3; // đã tồn tại ExamSubmissionDetails với ExamSubmissionId và QuestionId này
            //}

            //kiểm tra ChosenAnswer có hợp lệ không
            //if (studentChosenDetail.ChosenAnswer != null)
            //{
            //    if (!Enum.IsDefined(typeof(Constants.Enums.Choice), studentChosenDetail.ChosenAnswer))
            //    {
            //        return -4; // ChosenAnswer không hợp lệ
            //    }
            //}


            var examSubmissionDetails = _mapper.Map<ExamSubmissionDetail>(studentChosenDetail);
            examSubmissionDetails.IsCorrect = question.CorrectAnswer == studentChosenDetail.ChosenAnswer;
            examSubmissionDetails.ExamSubmissionId = examSubmissionId;
            _context.ExamSubmissionDetails.Add(examSubmissionDetails);
            _context.SaveChanges();
            return examSubmissionDetails.Id; // trả về Id của ExamSubmissionDetails vừa tạo
        }
    }
}
