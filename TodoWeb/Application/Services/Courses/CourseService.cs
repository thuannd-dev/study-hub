using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Courses
{
    public class CourseService : ICourseService
    {
        //thêm thuộc tính IApplicationDbContext vào class, và khỏi tạo giá trị thông qua constructer để 
        //từ đó class có phiên làm việc với cơ sở dữ liệu cho riêng mình

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CourseService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<CourseViewModel> GetCourses(int? courseId)//ở đây không phải hiểu là hàm có nhận vào giá trị hay không đều được
                                                                     //mà phải hiểu là hàm nhận vào giá trị khác null hoặc là null
        {

            //build lên một câu query 
            var query = _context.Course.AsQueryable();
            //var query = _context.Course.AsNoTracking().AsQueryable(); asnotracking thì tk entity nó sẽ ko check cái entity này nữa
            //=> tăng performance cho app nhưng khi modified sẽ không lưu được xuống database
            //nếu courseId có giá trị thì lấy ra đúng course với id đó
            if (courseId.HasValue)
            {
                query = query.Where(course => course.Id == courseId);
                if (query.Count() == 0) return null;
            }
            var result = _mapper.ProjectTo<CourseViewModel>(query).ToList();

            //List<Course> course = query.ToList();

            ////var result = course.Select(course => _mapper.Map<CourseViewModel>(course))
            ////    .ToList();

            //var result = _mapper.Map<List<CourseViewModel>>(course);



            return result;

            //return query.Where(course => course.Status != Constants.Enums.Status.Deleted)
            //    .Select(course => new CourseViewModel
            //{
            //    Id = course.Id,
            //    Name = course.Name,
            //    StartDate = course.StartDate,
            //}).ToList();

        }

        public async Task<int> Post(PostCourseViewModel course)
        {
            //kiểm tra xem name có bị trùng hay không
            var dupCourseName = await _context.Course.FirstOrDefaultAsync(c => c.Name == course.CourseName);
            //var dupCourseName2 = await _context.Course.FirstOrDefaultAsync(c => c.Name == course.CourseName);
            //db context là thread safe nên ko thể cho nó chạy song song
            if (dupCourseName != null) return -1;


            var data = _mapper.Map<Course>(course);
            _context.Course.Add(data);
            //ko await thì sẽ có khả năng lỗi
            await _context.SaveChangesAsync();
            return data.Id;
        }

        public int Put(CourseViewModel course)//src
        {
            //kiểm tra xem có id hay không
            var oldCourse = _context.Course.Find(course.CourseId);
            if (oldCourse == null || oldCourse.Status == Constants.Enums.Status.Deleted)
            {
                return -1;
            }
            //kiểm tra xem name có bị trùng hay không
            var dupCourseName = _context.Course.FirstOrDefault(c => c.Name == course.CourseName);
            if (dupCourseName != null) return -1;
            //thay doi 
            //oldCourse.Name = course.CourseName;
            //oldCourse.StartDate = course.StartDate;

            _mapper.Map(course, oldCourse);

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
            query = query.Where(course => course.Status != Constants.Enums.Status.Deleted)
                .Include(course => course.CourseStudent)
                .ThenInclude(courseStudent => courseStudent.Student);
            //return query.Select(course => new CourseStudentDetailViewModel
            //{
            //    CourseId = course.Id,
            //    CourseName = course.Name,
            //    StartDate = course.StartDate,
            //    Students = course.CourseStudent.Select(courseStudent => new Dtos.StudentModel.StudentViewModel
            //    {
            //        Id = courseStudent.Student.Id,
            //        FullName = $"{courseStudent.Student.FirstName} {courseStudent.Student.LastName}",
            //        Age = courseStudent.Student.Age,
            //        Balance = courseStudent.Student.Balance,
            //        SchoolName = courseStudent.Student.School.Name,
            //    }).ToList()
            //});
            return _mapper.ProjectTo<CourseStudentDetailViewModel>(query);
        }
    }
}
