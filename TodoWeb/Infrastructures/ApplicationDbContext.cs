using Microsoft.EntityFrameworkCore;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures.DatabaseMapping;

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

        //constructer
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ 
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-TUDP88B\\SQLEXPRESS;Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=True");
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

            modelBuilder.ApplyConfiguration(new StudentMapping());
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new CourseStudentMapping());
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new GradeMapping());
            base.OnModelCreating(modelBuilder);

            
                
        }

        public int SaveChange()
        {
            return base.SaveChanges();
        }

        
    }
}
