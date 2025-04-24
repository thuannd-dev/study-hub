using Microsoft.Data.SqlClient.DataClassification;

namespace TodoWeb.Application.Dtos.StudentModel
{
    public class StudentPagingViewModel
    {
        public IEnumerable<StudentViewModel> Students { get; set; }

        public int TotalPages { get; set; }
    }
}
