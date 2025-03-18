using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.ToDoModel;
using TodoWeb.Application.Services;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services
{
    public interface IToDoService
    {
        int Post(ToDoViewModel toDo);

        Guid Generate();
    }

    public class ToDoService : IToDoService
    {

        private readonly IApplicationDbContext _dbContext;
        private readonly IGuidGenerator _guidGenerator;
        public ToDoService(IApplicationDbContext dbContext, IGuidGenerator guidGenerator)
        {
            _dbContext = dbContext;
            _guidGenerator = guidGenerator;
        }

        public Guid Generate()
        {
            return _guidGenerator.Generate();
        }

        public int Post(ToDoViewModel toDo)
        {
            var data = new ToDo
            {
                Description = toDo.Description,
            };
            _dbContext.ToDos.Add(data);//add lúc này chỉ lưu trên memmory thôi, chúng ta phải sử dụng savechange để lưu xuống database
            _dbContext.SaveChanges();
            return data.Id;
        }
    }
}
