namespace TodoWeb.Application.Dtos.GradeStudentModel
{
    public class CourseGradeViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal? AssignmentScore { get; set; }
        public decimal? PracticalScore { get; set; }
        public decimal? FinalScore { get; set; }

        //diem so trung binh cua mon hoc
        
        public decimal? AverageScore
        {
            get
            {
                if (AssignmentScore.HasValue && PracticalScore.HasValue && FinalScore.HasValue)
                {
                    return Math.Round((decimal)((AssignmentScore + PracticalScore + FinalScore) / 3), 2);
                }
                return null;
            }
        }

    }
}
