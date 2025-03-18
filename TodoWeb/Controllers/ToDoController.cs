using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.Dtos.GuidModel;
using TodoWeb.Application.Dtos.ToDoModel;
using TodoWeb.Application.Services;
using TodoWeb.Application.Services.Students;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IToDoService _todoService;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ISingletonGenerator _singletonGenerator;
        private readonly GuidData _guidData;
        public ToDoController(
            IApplicationDbContext dbContext,//Tk dbcontext lúc này sẽ tự new constructer của applicationdbcontext (do đã register đã program nên .net sẽ tự hiểu)
            IToDoService todoService,
            IGuidGenerator guidGenerator,
            ISingletonGenerator singletonGenerator,
            GuidData guidData) 
        {
            _dbContext = dbContext;
            _todoService = todoService;
            _guidGenerator = guidGenerator;
            _singletonGenerator = singletonGenerator;
            _guidData = guidData;
        }

        [HttpGet("guid")] //Attribute de declare method nay la mot http method get
        public Guid[] GetGuid()
        {
            _guidData.guidGenerator = _guidGenerator;   
            return
                new Guid[]
                {
                _guidData.GetGuid(),//transient
                _singletonGenerator.Generate(),//
                };
        }

        [HttpGet]
        public IEnumerable<ToDoViewModel> Get(bool isCompleted)
        {
            var data = _dbContext.ToDos.Where(x => x.IsCompleted == isCompleted)
                .Select(x => new ToDoViewModel
                { 
                    Description = x.Description,
                    IsCompleted = x.IsCompleted
                }).ToList();

            return data;

        }

        [HttpPost]
        public int Post(ToDoViewModel toDo)
        {
            //var data = new ToDo
            //{
            //    Description = toDo.Description,
            //};
            //_dbContext.ToDos.Add(data);
            //_dbContext.SaveChanges();
            //return data.Id;

            return _todoService.Post(toDo);
        }

        [HttpPut]
        public int Put(ToDoUpdateModel toDo) // khoong truyeenf vvề cả todo bởi vì làm như vậy khiến tập tin rất lớn và k cần thiết
        {
            var data = _dbContext.ToDos.Find(toDo.Id);
            if (data == null)
            {
                return -1;
            }
            data.Description = toDo.Description;
            data.IsCompleted = toDo.IsCompleted;
            _dbContext.SaveChanges();
            return toDo.Id;
        }

        [HttpDelete]
        public int Delete(int id)
        {
            var data = _dbContext.ToDos.Find(id);
            if (data == null)
            {
                return -1;
            }
            _dbContext.ToDos.Remove(data);
            _dbContext.SaveChanges();
            return 0;
        }
    }
}
