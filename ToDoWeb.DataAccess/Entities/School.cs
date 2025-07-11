using TodoWeb.Constants.Enums;
using ToDoWeb.DataAccess.Entities;

namespace TodoWeb.Domains.Entities
{
    public class School : IEntity, IDelete
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public Status Status { get; set; }
        public Role? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
