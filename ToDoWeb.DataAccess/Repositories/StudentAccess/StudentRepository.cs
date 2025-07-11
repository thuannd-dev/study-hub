using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;
using ToDoWeb.DataAccess.Repositories.GenericAccess;
namespace ToDoWeb.DataAccess.Repositories.StudentAccess
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
