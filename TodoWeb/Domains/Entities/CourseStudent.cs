namespace TodoWeb.Domains.Entities
{
    public class CourseStudent
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
        public int StudentId { get; set; }

        public Student Student { get; set; }
        public Grade Grade { get; set; }

    }
}
