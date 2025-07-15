using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoWeb.Domains.Entities;
using ToDoWeb.Service.Services;
namespace CourseServiceTest
{
    public class ExampleServiceTest
    {
        [Fact]
        public async Task CreateCourseWithCurrentTime_ShouldReturnCorrectMessage()
        {
            //Arrange:
            var exampleService = new ExampleService();

            var courseName = "Test Course";
            var currentTime = DateTime.Now;
            //Act:
            var result = exampleService.CreateCourseWithCurrentTime(courseName);
            //Assert:
            var expectedMessage = $"Course '{courseName}' created at {currentTime:yyyy-MM-dd HH-mm-ss-FFFFF}";
            Assert.Equal(expectedMessage, result);
            //DateTime.Now ở hai bên lúc này sẽ khác nhau
            //Có hai cách fix một là biến currentTime trở thành param trong service, khi dùng thì truyền vào 
            //=> sẽ làm cho hai bên xài chung một instance

            //Cách 2: Dùng interface IsystemClock để mock (tạo instance mock)


            //Cách 3:
            /*
             Định nghĩa phương thức ảo

                protected virtual DateTime GetCurrentTime() => DateTime.UtcNow;
                – Cho phép các lớp con ghi đè (override).
            Sử dụng trong service

     
                var currentTime = GetCurrentTime();
                // … logic tạo Course và format chuỗi …
            Tạo lớp test kế thừa và override
                public class TestableExampleService : ExampleService
                {
                    protected override DateTime GetCurrentTime() => fixedTime;
                }
                – Khi gọi CreateCourseWithCurrentTime(), CLR sẽ chạy GetCurrentTime() của lớp con, trả về thời gian cố định.
             Kết quả
                – Production: GetCurrentTime() trả về DateTime.UtcNow.
                – Unit Test: Trả về fixedTime do bạn đặt, đảm bảo kết quả so sánh chuỗi luôn ổn định.
             */
        }
    }
}
