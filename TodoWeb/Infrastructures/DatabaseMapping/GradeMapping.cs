using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class GradeMapping : IEntityTypeConfiguration<Grade>
    {
        //sau khi tao config can phai apply config
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasKey(grade => grade.Id);

            builder.HasOne<CourseStudent>(grade => grade.CourseStudent)
                .WithOne(courseStudent => courseStudent.Grade)
                .HasForeignKey<Grade>(grade => grade.CourseStudentId);

            builder.Property(grade => grade.AssignmentScore).IsRequired(false).HasColumnType("decimal(5,2)");//Hoi tai sao 18,2
            builder.Property(grade => grade.PracticalScore).IsRequired(false).HasColumnType("decimal(5,2)");
            builder.Property(grade => grade.FinalScore).IsRequired(false).HasColumnType("decimal(5,2)");
        }
    }
}
