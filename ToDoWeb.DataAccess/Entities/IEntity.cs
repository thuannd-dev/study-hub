using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoWeb.DataAccess.Entities
{
    public interface IEntity
    {
        public int Id { get; set; }
    }
}
