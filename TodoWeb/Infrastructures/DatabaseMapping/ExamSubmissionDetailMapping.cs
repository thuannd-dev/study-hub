using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class ExamSubmissionDetailMapping : IEntityTypeConfiguration<ExamSubmissionDetail>
    {
        public void Configure(EntityTypeBuilder<ExamSubmissionDetail> builder)
        {
            // Cấu hình cho ExamSubmissionDetail: mối quan hệ giữa ExamSubmission và Question
            builder.HasKey(esd => esd.Id);

            builder.HasOne(esd => esd.ExamSubmission)
                .WithMany(es => es.ExamSubmissionDetails)
                .HasForeignKey(esd => esd.ExamSubmissionId);

            builder
                .HasOne(esd => esd.Question)
                .WithMany() //Question có thể được tham chiếu bởi nhiều ExamSubmissionDetail
                .HasForeignKey(esd => esd.QuestionId);




        }
    }
}
