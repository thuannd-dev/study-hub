namespace TodoWeb.Application.Dtos.GradeStudentModel
{
    public class StudentCourseGradeWithAverageCourseScoreViewModel
    {
        public StudentCourseGradeViewModel StudentCourseGradeViewModel { get; set; }
        public decimal? AverageCoursesScore
        {
            get
            {
                if (StudentCourseGradeViewModel.CourseScore == null || StudentCourseGradeViewModel.CourseScore.Count() == 0)
                {
                    return null;
                }
                return StudentCourseGradeViewModel.CourseScore.Average(c => c.AverageScore);
            }
        }
    }
}
