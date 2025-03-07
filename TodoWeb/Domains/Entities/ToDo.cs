using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Domains.Entities
{
    //cau truc cua mot dong trong table ToDos
    public class ToDo
    {
        [Key]//Data Annotations
        public int Id { get; set; }

        public string Description { get; set; }
        public bool IsCompleted { get; set; }


    }
}
