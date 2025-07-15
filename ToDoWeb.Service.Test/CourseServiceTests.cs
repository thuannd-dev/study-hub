using AutoMapper;
using Moq;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Domains.Entities;
using ToDoWeb.DataAccess.Repositories.CourseAccess;

namespace ToDoWeb.Service.Test
{
    public class CourseServiceTests
    {
        //method name  - condition - expected behavior
        [Fact]
        public void Post_WithNonExistentCourse_ReturnCreatedId()
        {
            var inputCourse = new PostCourseViewModel
            {
                CourseName = "New Course",
                StartDate = DateTime.UtcNow
            };

            var courseRepositoryMock = new Mock<ICourseRepository>();
            courseRepositoryMock
                .Setup(repo => repo.GetCourseByNameAsync(It.IsAny<String>()))
                .ReturnsAsync((Course?)null);

            var mMock = new Mock<IMapper>();
            mMock
                .Setup(m => m.Map<Course>(It.IsAny<PostCourseViewModel>()))
                .Returns(new Course { CourseName = inputCourse.CourseName, StartDate = inputCourse.StartDate });
        }
    }
}