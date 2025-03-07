using Microsoft.EntityFrameworkCore;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures
{
    public interface IApplicationDbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public int SaveChanges();

    }
}
