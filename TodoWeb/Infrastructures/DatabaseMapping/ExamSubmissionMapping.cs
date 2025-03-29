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
