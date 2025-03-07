using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Course
{
    public class CourseService : ICourseService
    {
        //thêm thuộc tính IApplicationDbContext vào class, và khỏi tạo giá trị thông qua constructer để 
        //từ đó class có phiên làm việc với cơ sở dữ liệu cho riêng mình

        private readonly IApplicationDbContext _context;

        public CourseService(IApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<CourseViewModel> GetCourses(int? courseId)//ở đây không phải hiểu là hàm có nhận vào giá trị hay không đều được
                                                                     //mà phải hiểu là hàm nhận vào giá trị khác null hoặc là null
        {

            //build lên một câu query 
            var query = _context.Course.AsQueryable();
            //nếu courseId có giá trị thì lấy ra đúng course với id đó
            if (courseId.HasValue)
            {
                query = query.Where(course => course.Id == courseId);
                if (query.Count() == 0) return null;
            }
            return query.Select(course => new CourseViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                StartDate = course.StartDate,
            }).ToList();

        }

        public int Post(PostCourseViewModel course)
        {
            //kiểm tra xem name có bị trùng hay không
            var dupCourseName = _context.Course.FirstOrDefault(c => c.Name == course.CourseName);
            if (dupCourseName != null) return -1;
            //tạo ra instance of new course
            var data = new Domains.Entities.Course
            {
                Name = course.CourseName,
                StartDate = course.StartDate,
            };
            _context.Course.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public int Put(CourseViewModel course)
        {
            //kiểm tra xem có id hay không
            var oldCourse = _context.Course.Find(course.CourseId);
            if (oldCourse == null)
            {
                return -1;
            }
            //kiểm tra xem name có bị trùng hay không
            var dupCourseName = _context.Course.FirstOrDefault(c => c.Name == course.CourseName);
            if (dupCourseName != null) return -1;
            //thay doi 
            oldCourse.Name = course.CourseName;
            oldCourse.StartDate = course.StartDate;
            _context.SaveChanges();
            return oldCourse.Id;
        }

        public int Delete(int courseId)
        {
            //kiểm tra xem có id hay không
            var oldCourse = _context.Course.Find(courseId);
            if (oldCourse == null)
            {
                return -1;
            }
            _context.Course.Remove(oldCourse);
            _context.SaveChanges();
            return oldCourse.Id;
        }

        public IEnumerable<CourseStudentDetailViewModel> GetCoursesDetail(int? courseId)
        {
            //build lên một câu query 
            var query = _context.Course.AsQueryable();
            //nếu courseId có giá trị thì lấy ra đúng course với id đó
            if (courseId.HasValue)
            {
                query = query.Where(course => course.Id == courseId);
                if (query.Count() == 0) return null;
            }
            //join
            query = query.Include(course => course.CourseStudent)
                .ThenInclude(courseStudent => courseStudent.Student);
            return query.Select(course => new CourseStudentDetailViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                StartDate = course.StartDate,
                Students = course.CourseStudent.Select(courseStudent => new Dtos.StudentModel.StudentViewModel
                {
                    Id = courseStudent.Student.Id,
                    FullName = $"{courseStudent.Student.FirstName} {courseStudent.Student.LastName}",
                    Age = courseStudent.Student.Age,
                    Balance = courseStudent.Student.Balance,
                    SchoolName = courseStudent.Student.School.Name,
                }).ToList()
            });
        }
    }
}
