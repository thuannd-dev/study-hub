using AutoMapper;
using TodoWeb.Application.Dtos.CourseModel;
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
    }
}
