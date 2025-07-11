using Microsoft.EntityFrameworkCore;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

using ToDoWeb.DataAccess.Repositories.GenericAccess;
namespace ToDoWeb.DataAccess.Repositories.SchoolAccess
{
    public class SchoolRepository : GenericRepository<School>, ISchoolRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SchoolRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<School?> GetSchoolsByNameAsync(string schoolName)
        {
            return await _dbContext.School.SingleOrDefaultAsync(s => s.Name.Equals(schoolName));
        }

    }
}
