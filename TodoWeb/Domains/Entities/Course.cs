namespace TodoWeb.Domains.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        //hỏi
        public ICollection<CourseStudent> CourseStudent { get; set; }
    }
}
