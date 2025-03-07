using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class CourseStudentMapping : IEntityTypeConfiguration<CourseStudent>
    {
        public void Configure(EntityTypeBuilder<CourseStudent> builder)
        {
            builder.HasKey(courseStudent => courseStudent.Id);

            builder.HasIndex(courseStudent => new { courseStudent.CourseId, courseStudent.StudentId })
                .IsUnique();

        }
    }
}
