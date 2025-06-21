using TodoWeb.Application.Dtos.ExamSubmissionsModel;

namespace TodoWeb.Application.Services.ExamSubmissions
{
    public interface IExamSubbmissionService
    {
        public int CreateStudentExamSubmission(StudentExamSubmissionCreateModel newStudentExamSubmission);
    }
}
