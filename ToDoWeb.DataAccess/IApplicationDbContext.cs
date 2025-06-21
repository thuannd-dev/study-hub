using Microsoft.EntityFrameworkCore;
using TodoWeb.Domains.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamSubmission> ExamSubmissions { get; set; }
        public DbSet<ExamSubmissionDetail> ExamSubmissionDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public EntityEntry<T> Entry<T>(T entity) where T : class;
        public int SaveChanges();
        public Task<int> SaveChangesAsync();

    }
}
