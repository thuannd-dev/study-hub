using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class ExamSubmissionMapping : IEntityTypeConfiguration<ExamSubmission>
    {
        //Cấu hình cho ExamSubmission: mối quan hệ giữa Exam và CourseStudent
        public void Configure(EntityTypeBuilder<ExamSubmission> builder)
        {

            //FinalScore
            builder.Property(x => x.FinalScore)
                .HasPrecision(5, 2); // Độ chính xác của số thập phân
                                     //.HasComputedColumnSql("10 * (SELECT COUNT(*) FROM ExamSubmissionDetails WHERE ExamSubmissionId = Id AND IsCorrect = 1) / (SELECT COUNT(*) FROM ExamSubmissionDetails WHERE ExamSubmissionId = Id)");
                                     //Ném ra lỗi "Subqueries are not allowed in this context. Only scalar expressions are allowed."
                                     //Bởi vì computed column không hỗ trợ subquery, chỉ hỗ trợ các biểu thức đơn giản hoặc hàm scalar
                                     //======> Có hai hướng đi
                                     //Sử dụng controller để tính toán giá trị của FinalScore khi người dùng put, post or delete bên examsubmissiondetail => phức tạp
                                     //Sử dụng trigger trong SQL Server để tự động tính toán giá trị của FinalScore khi có sự thay đổi trong ExamSubmissionDetails => đơn giản hơn
                                     //Nhưng bởi vì requirement chỉ yêu cầu submission nên mình sẽ không làm trigger, mình làm controller =)))
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Exam)
                .WithMany(x => x.ExamSubmissions)
                .HasForeignKey(x => x.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(es => es.CourseStudent)
            .WithMany(cs => cs.ExamSubmissions)
            .HasForeignKey(es => es.CourseStudentId);
        }
    }
}
