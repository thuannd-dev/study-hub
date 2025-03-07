using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWeb.Application.Dtos.GradeStudentModel
{
    public class StudentCourseGradeViewModel
    {
        public int StudentId { get; set; }
        public String StudentName { get; set; }
        public IEnumerable<CourseGradeViewModel> CourseScore { get; set; }
        
    }
}
