using System.Linq.Expressions;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;
using TodoWeb.Constants.Enums;
using Microsoft.EntityFrameworkCore;
namespace ToDoWeb.DataAccess.Repositories.StudentAccess
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public StudentRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(int? studentId,
            Expression<Func<Student, object>>? expression)
        {
            var query = _dbContext.Students
                .Where(student => student.Status != Status.Deleted)
                .AsQueryable();
            if (studentId.HasValue)
            {
                query = query.Where(x => x.Id == studentId);
            }
            if (expression != null)
            {
                query = query.Include(expression);
            }
            return await query.ToListAsync();
        }
    }
}
