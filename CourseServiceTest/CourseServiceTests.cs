using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TodoWeb.Application.Dtos.CourseModel;
using TodoWeb.Application.Services.Courses;
using TodoWeb.Domains.Entities;
using ToDoWeb.DataAccess.Repositories.CourseAccess;
using ToDoWeb.Service.Dtos.CourseModel;
namespace CourseServiceTest
{
    public class CourseServiceTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CourseService _courseService; 
        //goị constructor của class để khởi tạo giá trị cho các field mỗi lần gọi hàm test
        public CourseServiceTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _mapperMock = new Mock<IMapper>();
            _courseService = new CourseService(_mapperMock.Object, _courseRepositoryMock.Object); // Use correct field names
        }

        [Fact]
        public async Task PostAsync_WithNoneExictentCourse_ReturnscreatedCourseId()
        {
            // Arrange: set up the neccessary objects and mocks for the test
            var inputCourse = new PostCourseViewModel
            {
                CourseName = "New Course",
                StartDate = DateTime.UtcNow
            };

            var expectedCourse = new Course
            {
                Id = 9999,
                Name = inputCourse.CourseName,
                StartDate = inputCourse.StartDate
            };

            _courseRepositoryMock
                .Setup(repo => repo.GetCourseByNameAsync(inputCourse.CourseName))
                .ReturnsAsync((Course?)null); 


            _mapperMock.Setup(mapper => mapper.Map<Course>(inputCourse))
                .Returns(expectedCourse);

            _courseRepositoryMock
                .Setup(repo => repo.AddAsync(expectedCourse))
                .ReturnsAsync(expectedCourse.Id);

            //Act: call the method under test
            var result =  await _courseService.PostAsync(inputCourse);

            // Assert: verify the result and interactions with mocks
            Assert.Equal(expectedCourse.Id, result);
            Assert.NotEqual(0, result); // Ensure a valid ID is returned
            _mapperMock.Verify(mapper => mapper.Map<Course>(inputCourse), Times.Once);
            _courseRepositoryMock.Verify(repo => repo.AddAsync(expectedCourse), Times.Once);
            _courseRepositoryMock.Verify(repo => repo.GetCourseByNameAsync(inputCourse.CourseName), Times.Once);
        }


        [Fact]
        //attribute để đánh dấu phương thức là một test case
        //naming convention: tên method_case-condition_expected result
        public async Task PostAsync_WithExistingCourse_ThrowInvalidOperationException()
        {
            const string existingCourseName = "Existing Course";
            // Arrange: set up the necessary objects and mocks for the test
            var inputCourse = new PostCourseViewModel
            {
                CourseName = existingCourseName,
                StartDate = DateTime.UtcNow
            };

            var existingCourse = new Course
            {
                Id = 999,
                Name = existingCourseName,
                StartDate = inputCourse.StartDate
            };

            _courseRepositoryMock
                .Setup(repo => repo.GetCourseByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(existingCourse);

            _mapperMock
                .Setup(mapper => mapper.Map<Course>(inputCourse))
                .Returns(existingCourse);

            _courseRepositoryMock
                .Setup(repo => repo.AddAsync(existingCourse))
                .ReturnsAsync(existingCourse.Id);

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _courseService.PostAsync(inputCourse)); // Ensure exception is awaited
            //Assert là validate lại kết quả của hàm có trả về đúng kết quả không
            //verify hai method nay chua tung duco call
            _mapperMock.Verify(m => m.Map<Course>(inputCourse), Times.Never);
            _courseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Course>()), Times.Never);
        }

        [Fact]
        public async Task PutAsync_WithValidInput_ReturnsUpdatedCourseId()
        {
            // Arrange
            var inputCourse = new CourseViewModel
            {
                CourseId = 1,
                CourseName = "Existing Course",
                StartDate = DateTime.UtcNow
            };

            var expectedCourse = new Course
            {
                Id = inputCourse.CourseId,
                Name = "Updated Course",
                StartDate = inputCourse.StartDate,
                Status = TodoWeb.Constants.Enums.Status.Verified

            };

            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(inputCourse.CourseId))
                .ReturnsAsync(expectedCourse);

            _mapperMock
                .Setup(mapper => mapper.Map(inputCourse, expectedCourse))
                .Verifiable();

            _courseRepositoryMock.Setup(repo => repo.UpdateAsync(expectedCourse))
                .Verifiable();

            //Act
            var id = await _courseService.Put(inputCourse);

            // Assert
            Assert.Equal(expectedCourse.Id, id);
            _courseRepositoryMock.Verify(repo => repo.UpdateAsync(expectedCourse), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map(inputCourse, expectedCourse), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetNonExistentOrDeletedCourses))]
        public async Task Put_WithNoneExstentOrDeletedCourse_ThrowsInvalidOperationException(Course? courseFromDb)
        {
            var inputCourse = new CourseViewModel
            {
                CourseId = 99999,
                CourseName = "Non-Existent Course",
                StartDate = DateTime.UtcNow
            };

            _courseRepositoryMock.Setup(repo => repo.GetByIdAsync(inputCourse.CourseId))
                .ReturnsAsync(courseFromDb);

            _mapperMock
                .Setup(mapper => mapper.Map(inputCourse, courseFromDb));

            _courseRepositoryMock
                .Setup(repo => repo.UpdateAsync(courseFromDb!))
                .Verifiable();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _courseService.Put(inputCourse));

            Assert.Equal($"Course with ID {inputCourse.CourseId} does not exist or has already been deleted.", exception.Message);

            _courseRepositoryMock.Verify(repo => repo.UpdateAsync(courseFromDb!), Times.Never);
            _mapperMock.Verify(mapper => mapper.Map(inputCourse, courseFromDb!), Times.Never);

        }
        [Fact]
        public async Task DeleteAsync_WithExistingCourse_ReturnsDeletedCourseId()
        {
            var existingCourseId = 1;
            var exsistingCourse = new Course
            {
                Id = existingCourseId,
                Name = "Existing Course",
                StartDate = DateTime.UtcNow,
                Status = TodoWeb.Constants.Enums.Status.Verified
            };

            _courseRepositoryMock
                .Setup(repo => repo.GetByIdAsync(existingCourseId))
                .ReturnsAsync(exsistingCourse);

            _courseRepositoryMock
                .Setup(repo => repo.DeleteAsync(exsistingCourse))
                .ReturnsAsync(existingCourseId);

            //Act  
            var deletedCourseId = await _courseService.Delete(existingCourseId);

            // Assert  
            Assert.Equal(existingCourseId, deletedCourseId);
            _courseRepositoryMock.Verify(repo => repo.GetByIdAsync(existingCourseId), Times.Once);
        } 

        public static IEnumerable<object[]> GetNonExistentOrDeletedCourses = new List<object[]>
        {
           new object[] { null },// Non-existent course
           new object[] { new Course { Status = TodoWeb.Constants.Enums.Status.Deleted } }// Deleted course
        };

    }
}