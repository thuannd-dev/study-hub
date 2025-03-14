using Microsoft.EntityFrameworkCore;
using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public interface ICreate
    {
        public Role CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
