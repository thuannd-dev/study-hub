using TodoWeb.Application.Dtos.CourseStudentDetailModel;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Services.Students
{
    public interface IStudentService
    {
        public IEnumerable<StudentViewModel> GetStudents(int? schoolId);
        public int Post(StudentViewModel student);
        public int Put(StudentViewModel student);
        public int Delete(int studentID);

        public StudentCourseDetailViewModel GetStudentDetails(int id);
    }
}
