using TodoWeb.Domains.Entities;
using ToDoWeb.DataAccess.Repositories.GenericAccess;

namespace ToDoWeb.DataAccess.Repositories.StudentAccess
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
    }
}
