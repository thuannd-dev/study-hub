using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class ExamQuestionMapping : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Exam)
                .WithMany(x => x.ExamQuestions)
                .HasForeignKey(x => x.ExamId);

            builder.HasOne(x => x.Question)
                .WithMany(x => x.ExamQuestions)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
