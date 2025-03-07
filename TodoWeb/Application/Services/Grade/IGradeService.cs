using TodoWeb.Application.Dtos.GradeStudentModel;

namespace TodoWeb.Application.Services.Grade
{
    public interface IGradeService
    {
        public IEnumerable<StudentCourseGradeViewModel> GetGradeOfStudents(int? StudentId, int? CourseId);
        public IEnumerable<StudentCourseGradeWithAverageCourseScoreViewModel> GetAverageGradeOfStudents(int? studentId);
        public int PostGradeStudent(PostGradeViewModel gradeViewModel);
        public int PutGradeStudent(PostGradeViewModel gradeViewModel);
    }
}
