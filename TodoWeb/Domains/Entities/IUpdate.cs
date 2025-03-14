using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public interface IUpdate
    {
        public Role? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
