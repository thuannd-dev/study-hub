using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Students
{


    public class StudentService : IStudentService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StudentService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Delete(StudentViewModel student)
        {
            throw new NotImplementedException();
        }

        //IQueryable cos khar nang build leen 1 cau query cos kha nang mo rong(extension)
        public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
        {
            //query = slect * from student 
            //join school on student.sId = School.id
            var query = _context.Students
                .Where(student => student.Status != Constants.Enums.Status.Deleted)
                .Include(student => student.School)//icludeQueryAble
                .AsQueryable();//build leen 1 cau query

            if (schoolId.HasValue)
            {
                //query = slect * from student 
                //join school on student.sId = School.id
                //Where(x => x.S.Id == schoolId)
                query = query.Where(x => x.School.Id == schoolId);//add theem ddk 
            }
            //Select 
            //Id = x.Id,
            //FullName = x.FirstName + x.LastName,
            //SchoolName = x.School.Name,
            //from student 
            //join school on student.schoolId = School.id
            //Where(x => x.SId == schoolId) (depend if schoolId is not null)
            var result = _mapper.ProjectTo<StudentViewModel>(query).ToList();
            return result;

            //return query.Select(x => new StudentViewModel
            //{
            //    Id = x.Id,
            //    FullName = x.FirstName + " " + x.LastName,
            //    Age = x.Age,
            //    SchoolName = x.School.Name,
            //}).ToList();//khi minhf chaams to list thi entity framework moi excute cau query 
            //chua to list thif se build tren memory
        }

        public int Post(StudentViewModel student)
        {
            //kiểm tra xem student id có bị trùng hay không
            var dupID = _context.Students.Find(student.Id);
            if (dupID != null || student.Id < 1)
            {
                return -1;
            }
            var name = student.FullName.Split(' ');
            //lấy school nhờ vào school name
            var school = _context.School.FirstOrDefault(s => s.Name.Equals(student.SchoolName));//không dùng where bởi vì tìm ra một list

            if (school == null)
            {
                return -1;
            }

            var data = new Domains.Entities.Student
            {
                Id = student.Id,
                FirstName = name[0],
                LastName = string.Join(" ", name.Skip(1)),
                SId = school.Id,
                School = school,

            };
            _context.Students.Add(data);
            _context.SaveChanges();
            return data.Id;

        }

        public int Put(StudentViewModel student)
        {
            //tìm student
            var data = _context.Students.Find(student.Id);
            if (data == null || data.Status == Constants.Enums.Status.Deleted)
            {
                return -1;
            }
            var name = student.FullName.Split(' ');
            //kiểm tra xem người dùng có đưa đúng tên school
            var school = _context.School.FirstOrDefault(s => s.Name.Equals(student.SchoolName));//không dùng where bởi vì tìm ra một list
            if (school == null)
            {
                return -1;
            }
            //data.FirstName = name[0];
            //data.LastName = string.Join(" ", name.Skip(1));
            //data.SId = school.Id;
            //data.School = school;
            //data.Balance = student.Balance;
            _mapper.Map(student, data);
            _context.SaveChanges();
            return data.Id;
        }

        public int Delete(int studentID)
        {
            var data = _context.Students.Find(studentID);
            if (data == null)
            {
                return -1;
            }
            _context.Students.Remove(data);
            _context.SaveChanges();
            return data.Id;
        }

        public StudentCourseDetailViewModel GetStudentDetails(int id)
        {
            var query = _context.Students
                .Where(student => student.Status != Constants.Enums.Status.Deleted)
                .Include(student => student.CourseStudent)
                .ThenInclude(cs => cs.Course);

            var student = query.FirstOrDefault(x => x.Id == id);//không dùng where bởi vì trả list
            //excute lúc này luôn, excute khi mình chấm cái gì đó mà kéo dữ liệu từ database thì nó sẽ excute 
            if (student == null)
            {
                return null;
            }
            //projectto dùng khi muốn map một câu query
            //còn map thì dùng khi muốn map một object
            return _mapper.Map<StudentCourseDetailViewModel>(student);

            //return new StudentCourseDetailViewModel
            //{
            //    StudentId = student.Id,
            //    StudentName = student.FirstName + " " + student.LastName,
            //    Courses = student.CourseStudent.Select(cs => new CourseViewModel
            //    {
            //        CourseId = cs.CourseId,
            //        CourseName = cs.Course.Name
            //    }).ToList()
            //};
        }
    }
}
