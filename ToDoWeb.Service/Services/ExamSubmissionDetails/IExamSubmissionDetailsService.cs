using TodoWeb.Application.Dtos.ExamSubmissionDetailsModel;
using TodoWeb.Application.Dtos.ExamSubmissionsModel;

namespace TodoWeb.Application.Services.ExamSubmissionDetails
{
    public interface IExamSubmissionDetailsService
    {
        public int CreateExamSubmissionDetails(ExamSubmissionDetailsCreateModel newExamSubmissionDetails);
        public int CreateStudentExamSubmissionDetails(int examSubmissionId, StudentChosenDetailModel studentChosenDetail);
    }
}
