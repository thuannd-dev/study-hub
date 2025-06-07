using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Services.Students
{
    public interface IStudentService
    {
        public IEnumerable<StudentViewModel> GetStudent(int? studentId);
        public IEnumerable<StudentViewModel> GetStudents();
        public StudentCourseDetailViewModel GetStudentDetails(int id);
        public StudentPagingViewModel GetStudents(int? schoolId, string? sortBy, bool isDescending, int? pageSize, int? pageIndex);
        public IEnumerable<StudentViewModel> SearchStudents(string searchTerm);
        public int Post(StudentViewModel student);
        public int Put(StudentViewModel student);
        public int Delete(int studentID);
    }
}
