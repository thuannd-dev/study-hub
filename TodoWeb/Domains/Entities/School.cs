namespace TodoWeb.Domains.Entities
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
