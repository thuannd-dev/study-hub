using TodoWeb.Application.Dtos.ExamModel;

namespace TodoWeb.Application.Services.Exams
{
    public interface IExamService
    {

        public IEnumerable<ExamViewModel> GetExams(int? examId, int? courseId);
        public int PostExam(ExamCreateModel newExam);
        public int PutExam(ExamUpdateModel updateExam);
        public int DeleteExam(int examId);
    }
}
