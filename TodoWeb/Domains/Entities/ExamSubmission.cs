using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class ExamSubmission: ICreate, IUpdate
    {
        public int Id { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        // Tham chiếu đến CourseStudent (người đăng ký khóa học)
        public int CourseStudentId { get; set; }
        public CourseStudent CourseStudent { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Điểm cuối được tính theo công thức: 10 * số câu đúng / tổng số câu
        public double? FinalScore { get; set; }
        public Role CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public Role? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        // Navigation property cho chi tiết bài làm
        public ICollection<ExamSubmissionDetail> ExamSubmissionDetails { get; set; }
        
    }   
}
