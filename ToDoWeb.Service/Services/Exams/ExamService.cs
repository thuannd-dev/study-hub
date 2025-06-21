using AutoMapper;
using TodoWeb.Application.Dtos.ExamModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Exams
{
    public class ExamService : IExamService
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExamService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<ExamViewModel> GetExams(int? examId, int? courseId)
        {
            IQueryable<Exam> query;
            if (examId.HasValue)
            {
                query = _context.Exams.Where(exam => exam.Id == examId && exam.Status != Constants.Enums.Status.Deleted);
            }
            else
            {
                query = _context.Exams.Where(exam => exam.Status != Constants.Enums.Status.Deleted);
            }

            if (courseId.HasValue)
            {
                query = query.Where(exam => exam.CourseId == courseId);
            }
            return _mapper.ProjectTo<ExamViewModel>(query);
        }

        public int PostExam(ExamCreateModel newExam)
        {
            // Check if the course exists
            var course = _context.Course.Find(newExam.CourseId);
            if (course == null || course.Status == Constants.Enums.Status.Deleted)
            {
                return -2;
            }

            var isNotValid = string.IsNullOrWhiteSpace(newExam.Name)
                          || newExam.StartTime.CompareTo(newExam.EndTime) >= 0;

            if (isNotValid)
            {
                return -1;
            }

            var exam = _mapper.Map<Exam>(newExam);
            _context.Exams.Add(exam);
            _context.SaveChanges();
            return exam.Id;
        }

        public int PutExam(ExamUpdateModel updateExam)
        {
            // Check if the exam exists
            var exam = _context.Exams.Find(updateExam.ExamId);
            if(exam == null || exam.Status == Constants.Enums.Status.Deleted)
            {
                return -1;
            }
            // Check if the course exists
            var course = _context.Course.Find(updateExam.CourseId);
            if (course == null || course.Status == Constants.Enums.Status.Deleted)
            {
                return -2;
            }

            var isNotValid = string.IsNullOrWhiteSpace(updateExam.Name)
                          || updateExam.StartTime.CompareTo(updateExam.EndTime) >= 0;

            if (isNotValid)
            {
                return -3;
            }
            _mapper.Map(updateExam, exam);
            _context.SaveChanges();
            return exam.Id;
        }
        public int DeleteExam(int examId)
        {
            // Check if the exam exists
            var exam = _context.Exams.Find(examId);
            if (exam == null || exam.Status == Constants.Enums.Status.Deleted)
            {
                return -1;
            }
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            return exam.Id;

        }
    }
}
