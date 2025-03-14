using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int CourseStudentId { get; set; }
        public decimal? AssignmentScore { get; set; }
        public decimal? PracticalScore { get; set; }
        public decimal? FinalScore { get; set; }
        
        public virtual CourseStudent CourseStudent { get; set; }

    }
}
