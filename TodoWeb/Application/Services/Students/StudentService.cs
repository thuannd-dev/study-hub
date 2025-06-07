using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Application.Extensions;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Students
{


    public class StudentService : IStudentService
    {
        private const string STUDENT_KEY = "StudentKey";
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public StudentService(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }
        public IEnumerable<StudentViewModel> GetStudent(int? studentId)
        {
            var query = _context.Students
                .Where(student => student.Status != Constants.Enums.Status.Deleted)
                .AsQueryable();//build leen 1 cau query
            if (studentId.HasValue)
            {
                query = query.Where(x => x.Id == studentId);//add theem ddk 
            }
            query = query.Include(student => student.School);
            var result = _mapper.ProjectTo<StudentViewModel>(query).ToList();
            return result;
        }

        public IEnumerable<StudentViewModel> GetStudents()
        {
            //var data = _cache.Get<IEnumerable<StudentViewModel>>(STUDENT_KEY);
            //if (data == null)
            //{
            //    data = GetAllStudent();
            //    var cacheOptions = new MemoryCacheEntryOptions()
            //        .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));//thoi gian ton tai trong cache
            //    _cache.Set(STUDENT_KEY, data, cacheOptions);
            //}
            //return data;
            var data = _cache.GetOrCreate(STUDENT_KEY, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(30);//sliding cập nhật lại thời gian ở mỗi lần request
                return GetAllStudent();
            });
            return data;
        }

        private IEnumerable<StudentViewModel> GetAllStudent()
        {
            var query = _context.Students
                .Where(student => student.Status != Constants.Enums.Status.Deleted)
                .Include(student => student.School)
                .AsQueryable();
            var result = _mapper.ProjectTo<StudentViewModel>(query).ToList();
            return result;
        }


        //IQueryable cos khar nang build leen 1 cau query cos kha nang mo rong(extension)
        //public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
        //{
        //    //query = slect * from student 
        //    //join school on student.sId = School.id
        //    var query = _context.Students
        //        .Where(student => student.Status != Constants.Enums.Status.Deleted)
        //        .Include(student => student.School)//icludeQueryAble
        //        .AsQueryable();//build leen 1 cau query

        //    if (schoolId.HasValue)
        //    {
        //        //query = slect * from student 
        //        //join school on student.sId = School.id
        //        //Where(x => x.S.Id == schoolId)
        //        query = query.Where(x => x.School.Id == schoolId);//add theem ddk 
        //    }
        //    //Select 
        //    //Id = x.Id,
        //    //FullName = x.FirstName + x.LastName,
        //    //SchoolName = x.School.Name,
        //    //from student 
        //    //join school on student.schoolId = School.id
        //    //Where(x => x.SId == schoolId) (depend if schoolId is not null)
        //    var result = _mapper.ProjectTo<StudentViewModel>(query).ToList();
        //    return result;

        //    //return query.Select(x => new StudentViewModel
        //    //{
        //    //    Id = x.Id,
        //    //    FullName = x.FirstName + " " + x.LastName,
        //    //    Age = x.Age,
        //    //    SchoolName = x.School.Name,
        //    //}).ToList();//khi minhf chaams to list thi entity framework moi excute cau query 
        //    //chua to list thif se build tren memory
        //}

        public StudentPagingViewModel GetStudents(int? schoolId, string? sortBy, bool isDescending, int? pageSize, int? pageIndex)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                return new StudentPagingViewModel
                {
                    Students = Enumerable.Empty<StudentViewModel>().ToList(),
                    TotalPages = 0
                };
            }
            var query = _context.Students
                .Where(student => student.Status != Constants.Enums.Status.Deleted)
                .Include(student => student.School)
                .AsQueryable();

            if (schoolId.HasValue)
            {
                query = query.Where(x => x.School.Id == schoolId);//add theem ddk 
            }


            // Map sortBy string into a list of Expression selectors  
            Expression<Func<Student, object>>[] sortSelectors;
            if (sortBy.IsNullOrEmpty())
            {
                sortSelectors = [];
            }else
            {
                sortSelectors = sortBy.ToLower()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => (Expression<Func<Student, object>>)(f.Trim() switch
                    {
                        "id" => student => student.Id,
                        "age" => student => student.Age,
                        "fullname" => student => student.FirstName + " " + student.LastName,
                        "schoolname" => student => student.School.Name,
                        "balance" => student => student.Balance,
                        _ => student => student.Id
                    })).ToArray();
            }

            if(pageSize.HasValue && pageIndex == null)
            {
                pageIndex = 1;
            }
            if (pageIndex.HasValue && pageSize == null)
            {
                pageSize = 5;
            }

            query = query
                .ApplySort(isDescending, sortSelectors)
                .ApplyPaging(pageIndex, pageSize)
                .AsQueryable();
            int totalPage;
            if(pageSize == null)
            {
                totalPage = 1;
            }else
            {
                var numberOfStudents = _context.Students.Count();
                totalPage = (int)Math.Ceiling((double)numberOfStudents / (int)pageSize);
            }
                
            var data = new StudentPagingViewModel
            {
                Students = _mapper.ProjectTo<StudentViewModel>(query).ToList(),
                TotalPages = totalPage
            };
            return data;
        }

        public IEnumerable<StudentViewModel> SearchStudents(string searchTerm)
        {
            var tokens = searchTerm
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var query = _context.Students
                .Where(student => student.Status != Constants.Enums.Status.Deleted)
                .Include(student => student.School)
                .AsQueryable();

            if (tokens.Length > 1)
            {
                query = query.ApplyRelatedSearch(searchTerm, student => student.Id.ToString(),
                    student => student.FirstName + " " + student.LastName,
                    student => student.Age.ToString(),
                    student => student.School.Name,
                    student => student.Balance.ToString());
            }else if (tokens.Length == 1)
            {
                query = query.ApplySearch(searchTerm, student => student.Id.ToString(),
                    student => student.FirstName + " " + student.LastName,
                    student => student.Age.ToString(),
                    student => student.School.Name,
                    student => student.Balance.ToString());
            }else
            {
                return Enumerable.Empty<StudentViewModel>();
            }

                var result = _mapper.ProjectTo<StudentViewModel>(query).ToList();
            return result;
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
            _cache.Remove(STUDENT_KEY);
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
