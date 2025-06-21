using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class ExamSubmissionDetail
    {
        public int Id { get; set; }

        public int ExamSubmissionId { get; set; }
        public ExamSubmission ExamSubmission { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        // Đáp án mà sinh viên chọn (A/B/C/D)
        public Choice? ChosenAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
