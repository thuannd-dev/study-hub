using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.CourseStudentModel;
using TodoWeb.Application.Services.CourseStudents;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.CourseStudents
{
    public class CourseStudentService : ICourseStudentService
    {
        //inject and use Auto Mapper
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CourseStudentService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int PostCourseStudent(PostCourseStudentViewModel courseStudentViewModel)
        {
            //kiem tra xem thu co studentId vaf courseId chuwa
            var hasStudentId = _context.Students.Find(courseStudentViewModel.StudentId);
            var hasCourseId = _context.Course.Find(courseStudentViewModel.CourseId);
            //kiem tra duplicate trong coursestudent
            var hasCourseStudent = _context.CourseStudent
                .FirstOrDefault(cs => cs.StudentId == courseStudentViewModel.StudentId && cs.CourseId == courseStudentViewModel.CourseId);
            if (hasCourseId != null && hasStudentId != null && hasCourseStudent == null)
            {
                //var data = new Domains.Entities.CourseStudent
                //{
                //    StudentId = courseStudentViewModel.StudentId,
                //    CourseId = courseStudentViewModel.CourseId
                //};
                var data = _mapper.Map<CourseStudent>(courseStudentViewModel);
                _context.CourseStudent.Add(data);
                _context.SaveChanges();
                return data.Id;
            }
            return -1;
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
