using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class CourseMapping : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(255);

            builder.Property(x => x.StartDate).HasDefaultValueSql("GETDATE()");

            builder.HasMany(course => course.CourseStudent)
                .WithOne(courseStudent => courseStudent.Course)
                .HasForeignKey(courseStudent => courseStudent.CourseId);
        }
    }
}
