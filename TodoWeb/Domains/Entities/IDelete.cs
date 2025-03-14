using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public interface IDelete
    {
        public Status Status { get; set; }
        public Role? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
