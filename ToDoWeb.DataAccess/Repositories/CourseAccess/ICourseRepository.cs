using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoWeb.Domains.Entities;
using ToDoWeb.DataAccess.Repositories.GenericAccess;

namespace ToDoWeb.DataAccess.Repositories.CourseAccess
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course?> GetCourseByNameAsync(string courseName);
        //Task<IEnumerable<Course>> GetAllAsync(int? id);
        //Task<Course?> GetByIdAsync(int id);
        //Task<int> AddAsync(Course course);
        //Task<int> UpdateAsync(Course course);
        //Task<int> DeleteAsync(Course course);
    }
}
