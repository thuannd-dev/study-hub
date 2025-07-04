using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoWeb.Domains.Entities;

namespace ToDoWeb.DataAccess.Repositories.StudentAccess
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync(int? studentId, Expression<Func<Student, object>>? include);
    }
}
