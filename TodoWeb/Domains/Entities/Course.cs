using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class Course : IUpdate, ICreate, IDelete
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        //hỏi
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
        public Role CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public Role? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Status Status { get; set; }
        public Role? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }

        internal Course? Where(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
