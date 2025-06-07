using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    [Table("Students")]
    public class Student : IDelete
    {
        [Key]//define primary key cua table, identity (ko the dinh nghia id)
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//none có nghĩa là khóa chính dễ dãi, cho làm mọi thứ, có thể chỉnh id
        public int Id { get; set; }
        
        [MaxLength(255)]//chieu dai toi da 255
        public string? FirstName { get; set; }//? giup co the null
        //[Column("Surname")]
        [StringLength(255)]
        public string? LastName { get; set; }

        //public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
        //[MaxLength(2000)]
        //public byte[] Image {  get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
        [ConcurrencyCheck]
        public decimal Balance { get; set; }
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [NotMapped]//khong duoc map xuong database (table khonmg co column nay)
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Age { get; set; }//data dua vao truong dateofbirth nen khong can set

        [ForeignKey("School")]
        public int SId { get; set; }
        public virtual School School { get; set; }

        public virtual ICollection<CourseStudent> CourseStudent { get; set; }

        public Status Status { get; set; }
        public Role? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
