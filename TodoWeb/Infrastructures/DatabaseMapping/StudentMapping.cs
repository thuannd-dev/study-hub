using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class StudentMapping : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            //Age
            builder.Property(x => x.Age)
                .HasComputedColumnSql("DATEDIFF(YEAR, DATEOFBIRTH, GETDATE())");
            //Relationship with CourseStudent
            builder.HasMany(student => student.CourseStudent)
                .WithOne(courseStudent => courseStudent.Student)
                .HasForeignKey(courseStudent => courseStudent.StudentId);
        }
    }
}
