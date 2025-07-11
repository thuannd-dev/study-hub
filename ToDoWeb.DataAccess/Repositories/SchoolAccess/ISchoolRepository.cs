using TodoWeb.Domains.Entities;
using ToDoWeb.DataAccess.Repositories.GenericAccess;

namespace ToDoWeb.DataAccess.Repositories.SchoolAccess
{
    public interface ISchoolRepository : IGenericRepository<School>
    {
        Task<School?> GetSchoolsByNameAsync(string schoolName);
    }
}
