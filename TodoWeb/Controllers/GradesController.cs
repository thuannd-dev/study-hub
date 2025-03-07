using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.Dtos.GradeStudentModel;
using TodoWeb.Application.Services.Grade;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradesController : Controller
    {
        //tao 1 instance cua gradeservice
        private readonly IGradeService _gradeService;
        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet("Student/{studentId}/Course/{courseId}")]
        public IEnumerable<StudentCourseGradeViewModel> GetGradeOfAStudentOfACourse(int studentId, int courseId)
        {
            return _gradeService.GetGradeOfStudents(studentId, courseId);
        }

        [HttpGet("Student/{studentId:int}")]
        public IEnumerable<StudentCourseGradeViewModel> GetGradeOfAStudent (int studentId)
        {
            return _gradeService.GetGradeOfStudents(studentId, null);
        }

        [HttpGet("Students/Course/{courseId}")]
        public IEnumerable<StudentCourseGradeViewModel> GetGradeOfAllStudentOfACourse(int courseId)
        {
            return _gradeService.GetGradeOfStudents(null, courseId)
                .Where(gradeStudents => gradeStudents.CourseScore.Count() != 0);
        }

        [HttpGet]
        public IEnumerable<StudentCourseGradeViewModel> GetGradeOfAllStudentOfAllCourse()
        {
            return _gradeService.GetGradeOfStudents(null, null);
        }

        [HttpGet("Student/AverageCoursesScore/{studentId}")]
        public IEnumerable<StudentCourseGradeWithAverageCourseScoreViewModel> GetAverageCourseScoreOfAStudent(int studentId)
        {
            return _gradeService.GetAverageGradeOfStudents(studentId);
        }
        
        [HttpGet("Students/AverageCoursesScore")]
        public IEnumerable<StudentCourseGradeWithAverageCourseScoreViewModel> GetAverageCourseScoreOfStudents()
        {
            return _gradeService.GetAverageGradeOfStudents(null);
        }

        [HttpPost]
        public int PostGradeStudent(PostGradeViewModel gradeViewModel)
        {
            return _gradeService.PostGradeStudent(gradeViewModel);
        }

        [HttpPut]
        public int PutGradeStudent(PostGradeViewModel gradeViewModel)
        {
            return _gradeService.PutGradeStudent(gradeViewModel);
        }
    }
}
