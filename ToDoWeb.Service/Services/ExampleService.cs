using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoWeb.Domains.Entities;

namespace ToDoWeb.Service.Services
{
    public class ExampleService
    {
        public string CreateCourseWithCurrentTime(string courseName)
        {
            var currentTime = DateTime.UtcNow;
            var course = new Course
            {
                Name = courseName,
                StartDate = currentTime,
                CreateAt = currentTime,
            };

            return $"Course '{course.Name}' created at {currentTime:yyyy-MM-dd HH-mm-ss-FFFFF}";
        }
    }
}
