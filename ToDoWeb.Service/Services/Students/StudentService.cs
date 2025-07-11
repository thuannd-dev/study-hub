using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Application.Extensions;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;
using ToDoWeb.DataAccess.Repositories.GenericAccess;
using ToDoWeb.DataAccess.Repositories.SchoolAccess;
using ToDoWeb.DataAccess.Repositories.StudentAccess;

namespace TodoWeb.Application.Services.Students
{

    public class StudentService : IStudentService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly ISchoolRepository _schoolRepository;
        public StudentService(IApplicationDbContext context, IGenericRepository<Student> studentRepository, ISchoolRepository schoolRepository,
            IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _studentRepository = studentRepository;
            _schoolRepository = schoolRepository;
        }

        public async Task<IEnumerable<StudentViewModel>> GetStudents(int? studentId)
        {
            var students = await _studentRepository.GetAllAsync(studentId, student => student.School);
            return _mapper.Map<IEnumerable<StudentViewModel>>(students);
        }

        public async Task<IEnumerable<StudentViewModel?>> GetAllStudents()
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
            //var data = await _cache.GetOrCreateAsync(STUDENT_KEY, async entry =>
            //{
            //    entry.SlidingExpiration = TimeSpan.FromSeconds(30);//sliding cập nhật lại thời gian ở mỗi lần request
            //    return await GetStudents(null);
            //});
            //return data;
            return await GetStudents(null);
        }

        public async Task<int> Post(StudentViewModel student)
        {
            //kiểm tra xem student id có bị trùng hay không
            var dupID = await _studentRepository.GetByIdAsync(student.Id);
            if (dupID != null || student.Id < 1)
            {
                throw new ArgumentException($"Student ID {student.Id} is invalid or already exists.");
            }
            var name = student.FullName.Split(' ');
            //lấy school nhờ vào school name
            var school = await _schoolRepository.GetSchoolsByNameAsync(student.SchoolName);

            if (school == null)
            {
                throw new ArgumentException($"School with name {student.SchoolName} does not exist.");
            }

            var data = new Domains.Entities.Student
            {
                Id = student.Id,
                FirstName = name[0],
                LastName = string.Join(" ", name.Skip(1)),
                SId = school.Id,
                School = school,

            };
            //_cache.Remove(STUDENT_KEY);
            return await _studentRepository.AddAsync(data);
        }

        public async Task<int> Put(StudentViewModel student)
        {
            //tìm student
            var data = await _studentRepository.GetByIdAsync(student.Id);
            if (data == null || data.Status == Constants.Enums.Status.Deleted)
            {
                throw new ArgumentException($"Student with ID {student.Id} does not exist or has been deleted.");
            }
            //kiểm tra xem người dùng có đưa đúng tên school
            var school = await _schoolRepository.GetSchoolsByNameAsync(student.SchoolName);//không dùng where bởi vì tìm ra một list
            if (school == null)
            {
                throw new ArgumentException($"School with name {student.SchoolName} does not exist.");
            }
            var name = student.FullName.Split(' ');
            data.FirstName = name[0];
            data.LastName = string.Join(" ", name.Skip(1));
            data.SId = school.Id;
            data.School = school;
            data.Balance = student.Balance;
            return await _studentRepository.UpdateAsync(data);
        }

        public async Task<int> Delete(int studentID)
        {
            var data = await _studentRepository.GetByIdAsync(studentID);
            if (data == null)
            {
                throw new ArgumentException($"Student with ID {studentID} does not exist.");
            }
            return await _studentRepository.DeleteAsync(data);
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
            }
            else
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

            if (pageSize.HasValue && pageIndex == null)
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
            if (pageSize == null)
            {
                totalPage = 1;
            }
            else
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
            }
            else if (tokens.Length == 1)
            {
                query = query.ApplySearch(searchTerm, student => student.Id.ToString(),
                    student => student.FirstName + " " + student.LastName,
                    student => student.Age.ToString(),
                    student => student.School.Name,
                    student => student.Balance.ToString());
            }
            else
            {
                return Enumerable.Empty<StudentViewModel>();
            }

            var result = _mapper.ProjectTo<StudentViewModel>(query).ToList();
            return result;
        }
    }
}
