using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoWeb.Domains.Entities;

namespace ToDoWeb.DataAccess.Repositories.CourseAccess
{
    public interface ICourseRepository
    {
        Task<int> AddCourseAsync(Course course);
        Task<int> DeleteCourseAsync(int courseId);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<Course?> GetCourseByNameAsync(string courseName);
        Task<IEnumerable<Course>> GetCoursesAsync(int? courseId);
        Task<int> UpdateCourseAsync(Course course);
    }
}
