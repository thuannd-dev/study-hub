using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Services.Students
{
    public interface IStudentService
    {
        public Task<IEnumerable<StudentViewModel>> GetStudents(int? studentId);
        public Task<IEnumerable<StudentViewModel?>> GetAllStudents();
        public StudentPagingViewModel GetStudents(int? schoolId, string? sortBy, bool isDescending, int? pageSize, int? pageIndex);
        public IEnumerable<StudentViewModel> SearchStudents(string searchTerm);
        public Task<int> Post(StudentViewModel student);
        public Task<int> Put(StudentViewModel student);
        public Task<int> Delete(int studentID);
    }
}
