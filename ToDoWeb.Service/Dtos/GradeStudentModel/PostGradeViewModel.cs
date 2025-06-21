using TodoWeb.Application.Dtos.CourseStudentModel;

namespace TodoWeb.Application.Dtos.GradeStudentModel
{
    public class PostGradeViewModel : PostCourseStudentViewModel
    {
        public decimal? AssignmentScore { get; set; }
        public decimal? PracticalScore { get; set; }
        public decimal? FinalScore { get; set; }
    }
}
