using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures.DatabaseMapping;
using TodoWeb.Infrastructures.Interceptor;

namespace TodoWeb.Infrastructures
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        //Table co ten la ToDos
        //moi dong la ToDo
        //DbSet<ToDo> nghia la mot table chua cac dong ToDo
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<School> School { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamSubmission> ExamSubmissions { get; set; }
        public DbSet<ExamSubmissionDetail> ExamSubmissionDetails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ModifyLoggingInterceptor> CreateUpdateLoggingInterceptors;
        //constructer
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ 
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer("Server=DESKTOP-TUDP88B\\SQLEXPRESS;Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=True");
            optionsBuilder.AddInterceptors(new SqlQueryLoggingInterceptor(), new AuditLoggingInterceptor(), new ModifyLoggingInterceptor());//add theo thứ tự nào thì code mình sẽ chạy theo thứ tự như thế đấy


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>()
            //    .Property(x => x.Age)
            //    .HasComputedColumnSql("DATEDIFF(YEAR, DATEOFBIRTH, GETDATE())");
            //modelBuilder.Entity<Student>()
            //    .HasMany(student => student.CourseStudent)
            //    .WithOne(courseStudent => courseStudent.Student)
            //    .HasForeignKey(courseStudent => courseStudent.StudentId);

            //modelBuilder.Entity<Course>()
            //    .HasMany(course => course.CourseStudent)
            //    .WithOne(courseStudent => courseStudent.Course)
            //    .HasForeignKey(courseStudent => courseStudent.CourseId);

            //modelBuilder.Entity<CourseStudent>()
            //    .HasKey(courseStudent => new { courseStudent.CourseId, courseStudent.StudentId });

            //set tất cả các foreign key đều là restrict thay vì là cascade delete

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfiguration(new StudentMapping());
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new CourseStudentMapping());
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new GradeMapping());
            modelBuilder.ApplyConfiguration(new ExamQuestionMapping());
            modelBuilder.ApplyConfiguration(new ExamSubmissionDetailMapping());
            modelBuilder.ApplyConfiguration(new ExamSubmissionMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            base.OnModelCreating(modelBuilder);

            
                
        }

        public int SaveChanges()
        {
            //var auditLogs = new List<AuditLog>();
            //foreach(var entity in ChangeTracker.Entries())
            //{
            //    var log = new AuditLog
            //    {
            //        EntityName = entity.Entity.GetType().Name,
            //        CreatedAt = DateTime.Now,
            //        Action = entity.State.ToString()
            //    };
            //    if(entity.State == EntityState.Added)
            //    {
            //        log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
            //    }
            //    if (entity.State == EntityState.Modified)
            //    {
            //        log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
            //        log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
            //    }
            //    if (entity.State == EntityState.Deleted)
            //    {
            //        log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
            //    }
            //    auditLogs.Add(log);

            //}
            //AuditLog.AddRange(auditLogs);//State =  Added
            return base.SaveChanges();
        }

        public EntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
