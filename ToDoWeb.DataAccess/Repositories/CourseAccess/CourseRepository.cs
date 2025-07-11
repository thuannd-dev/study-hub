using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;
using ToDoWeb.DataAccess.Repositories.GenericAccess;

namespace ToDoWeb.DataAccess.Repositories.CourseAccess
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        //private readonly IGenericRepository<Course> _genericRepo;
        public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// Có nên dùng async void hay không | Tại sao phải dùng async Task cho một hàm bất đồng bộ?
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
        //Task là một object chứa cái result của một asynchous function
        //Nếu success thì nó sẽ trả về một result- value trên cái Task đó
        //Nếu fail - exception thì nó sẽ store một exception trên cái Task đó luôn
        //======>>>>> Nếu dùng async void thì mình sẽ không có Task để mình await 
        //======>>>>> Vì vậy mình sẽ không thể biết được khi nào hàm đó hoàn thành
        //Bên cạnh đó khi nó throw exception thì mình cũng không thể catch được
        ////======>>> Exception sẽ pop up in call stack và nếu là windows application(winform) chỉ có một main thread thì sẽ làm ứng dụng bị crash
        //Còn thread trong web application nó chỉ là worker thread-ko có main thread => trở thành một unhandled exception,  nó sẽ không làm ứng dụng bị crash
        //======>>>>> Vì vậy không nên dùng async void trừ khi bạn đang viết một event handler(click button, form)
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Có thể dùng một function trả về một task
        //Sau đó dùng Task.WhenAll giúp các task chạy song song → nhanh hơn rất nhiều so với chờ từng cái (await từng cái một)
        //Tuy nhiên Cẩn thận khi dùng Task.WhenAll với EF Core
        //EF Core không thread-safe. Dùng cùng DbContext để chạy nhiều truy vấn song song sẽ gây lỗi runtime
        //Chỉ nên dùng Task.WhenAll khi các truy vấn không phụ thuộc vào nhau và không sử dụng chung DbContext
        //Nếu dùng cùng context thì await lần lượt từng truy vấn để tránh lỗi

        //public async Task<IEnumerable<Course>> GetCoursesAsync(int? courseId)
        //{
        //    // Build a query to get courses
        //    var query = _dbContext.Course.AsQueryable();
        //    // If courseId has a value, filter by that courseId
        //    if (courseId.HasValue)
        //    {
        //        query = query.Where(course => course.Id == courseId);
        //    }
        //    //if (!query.Any()) return Enumerable.Empty<Course>(); // Return empty Enumerable course if no courses found
        //    //return query.ToList(); // Return the list of courses

        //    //Làm như trên sẽ khiến truy vấn hai lần xuống database, vì any() sẽ thực hiện một truy vấn để kiểm tra xem có bản ghi nào không,
        //    //toList() sẽ thực hiện một truy vấn khác để lấy dữ liệu.
        //    //Thay vào đó, ta sử dụng ToList() để lấy tất cả các bản ghi và sau đó kiểm tra xem danh sách có rỗng hay không.
        //    //**Lưu ý : Chỉ any khi cần kiểm tra sự tồn tại của bản ghi mà không cần lấy dữ liệu sau đó, nếu cần lấy dữ liệu thì chỉ cần dùng ToList() hoặc ToArray() là đủ.**
        //    var courses = await query.ToListAsync(); // Get all courses as a list
        //    // Return empty Enumerable if no courses found, otherwise return the list of courses
        //    return courses.Count == 0 ? Enumerable.Empty<Course>() : courses;
        //}

        //public async Task<Course?> GetCourseByIdAsync(int courseId)
        //{
        //    //Find sẽ tìm trong context của DbContext trước, nếu không có thì mới truy vấn xuống database
        //    return await _dbContext.Course.FindAsync(courseId);
        //}

        public async Task<Course?> GetCourseByNameAsync(string courseName)
        {
            //Tìm kiếm khóa học theo tên
            return await _dbContext.Course
                .SingleOrDefaultAsync(c => c.Name == courseName);
        }

        //public async Task<int> AddCourseAsync(Course course)
        //{
        //    await _dbContext.Course.AddAsync(course);
        //    //Change tracker sẽ theo dõi các thay đổi của entity này, giá trị của id lúc
        //    //này là giá trị mặc định của kiểu dữ liệu int (0)
        //    await _dbContext.SaveChangesAsync();
        //    //Sau khi save changes, id sẽ được cập nhật với giá trị mới từ database
        //    return course.Id;

        //}

        //public async Task<int> UpdateCourseAsync(Course course)
        //{
        //    //Nên kiểm tra xem course có tồn tại trong database hay không trước khi cập nhật
        //    //Bởi vì mỗi operation nên independent với nhau, nếu chúng ta phụ thuộc vào nhau thì sẽ khó bảo trì-maintance hơn
        //    var existingCourse = await GetCourseByIdAsync(course.Id);
        //    if (existingCourse == null)
        //    {
        //        throw new ArgumentException($"Course with ID {course.Id} does not exist.");
        //    }
        //    //_dbContext.Course.Update(course);
        //    //cách-way này sẽ Đánh dấu toàn bộ entity course là Modified,
        //    //tức là EF sẽ update tất cả các field, bất kể-regardless có thay đổi hay không.
        //    _dbContext.Entry(existingCourse).CurrentValues.SetValues(course);
        //    // Chỉ cập nhật các trường đã thay đổi, giữ nguyên các trường không thay đổi
        //    return await _dbContext.SaveChangesAsync();
        //    //return number of state entries written to the database,
        //}

        //public async Task<int> DeleteCourseAsync(int courseId)
        //{
        //    var course = await GetCourseByIdAsync(courseId);
        //    if (course == null)
        //    {
        //        throw new ArgumentException($"Course with ID {courseId} does not exist.");
        //    }
        //    //_dbContext.Entry(course).State = EntityState.Deleted;
        //    //cách này sẽ đánh dấu toàn bộ entity course là Deleted, tức là EF sẽ xóa toàn bộ entity này khỏi database
        //    //Dùng khi xóa lượng lớn entities cùng lúc- bulk delete logic
        //    _dbContext.Course.Remove(course);
        //    //cách này sẽ đánh dấu entity course là Deleted, tức là EF sẽ xóa entity này khỏi database, dùng khi xóa một entity đơn lẻ
        //    await _dbContext.SaveChangesAsync();
        //    return courseId;

        //}
        //public async Task<int> UpdateCourseAsync(Course course)
        //{
        //    //Nên kiểm tra xem course có tồn tại trong database hay không trước khi cập nhật
        //    //Bởi vì mỗi operation nên independent với nhau, nếu chúng ta phụ thuộc vào nhau thì sẽ khó bảo trì-maintance hơn
        //    var existingCourse = await GetCourseByIdAsync(course.Id);
        //    if (existingCourse == null)
        //    {
        //        throw new ArgumentException($"Course with ID {course.Id} does not exist.");
        //    }
        //    //_dbContext.Course.Update(course);
        //    //cách-way này sẽ Đánh dấu toàn bộ entity course là Modified,
        //    //tức là EF sẽ update tất cả các field, bất kể-regardless có thay đổi hay không.
        //    _dbContext.Entry(existingCourse).CurrentValues.SetValues(course);
        //    // Chỉ cập nhật các trường đã thay đổi, giữ nguyên các trường không thay đổi
        //    return await _dbContext.SaveChangesAsync();
        //    //return number of state entries written to the database,
        //}

        //public async Task<int> DeleteCourseAsync(int courseId)
        //{
        //    var course = await GetCourseByIdAsync(courseId);
        //    if (course == null)
        //    {
        //        throw new ArgumentException($"Course with ID {courseId} does not exist.");
        //    }
        //    //_dbContext.Entry(course).State = EntityState.Deleted;
        //    //cách này sẽ đánh dấu toàn bộ entity course là Deleted, tức là EF sẽ xóa toàn bộ entity này khỏi database
        //    //Dùng khi xóa lượng lớn entities cùng lúc- bulk delete logic
        //    _dbContext.Course.Remove(course);
        //    //cách này sẽ đánh dấu entity course là Deleted, tức là EF sẽ xóa entity này khỏi database, dùng khi xóa một entity đơn lẻ
        //    await _dbContext.SaveChangesAsync();
        //    return courseId;

        //}

       // public Task<IEnumerable<Course>> GetAllAsync(int? id)
       //=> _genericRepo.GetAllAsync(id);

       // public Task<Course?> GetByIdAsync(int id)
       //     => _genericRepo.GetByIdAsync(id);

       // public Task<int> AddAsync(Course course)
       //     => _genericRepo.AddAsync(course);

       // public Task<int> UpdateAsync(Course course)
       //     => _genericRepo.UpdateAsync(course);

       // public Task<int> DeleteAsync(Course course)
       //     => _genericRepo.DeleteAsync(course);
    }  
}