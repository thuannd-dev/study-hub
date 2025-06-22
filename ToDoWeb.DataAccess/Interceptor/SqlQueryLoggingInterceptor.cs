using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TodoWeb.Infrastructures.Interceptor
{
    //log lại những câu query có thời gian thực thi lớn hơn 2 ms
    //
    public class SqlQueryLoggingInterceptor : DbCommandInterceptor
    {
        Stopwatch stopwatch = new Stopwatch();

        //trước excute 
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            stopwatch.Start();
            //do something 10s

            //var miliseconds = stopwatch.ElapsedMilliseconds; //10.000
            //using StreamWriter writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "sqllog.txt"), append: true);//append true có nghĩa là ghi đè
            //writer.WriteLine(command.CommandText);//command.CommnadText chính là cau sql của ban
            return base.ReaderExecuting(command, eventData, result);
        }

        //sau khi excute
        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            stopwatch.Stop();
            var miliseconds = stopwatch.ElapsedMilliseconds; //10.000
            if(miliseconds > 2)
            {
                //AppDomain.CurrentDomain.BaseDirectory không phải là thư mục gốc dự án, mà là thư mục thực thi (tức là thư mục chứa .exe khi chạy)
                //ví dụ: C:\Users\YourUserName\source\repos\YourProject\bin\Debug\net8.0\
                //nên nếu muốn ghi vào thư mục gốc dự án thì cần phải dùng Directory.GetCurrentDirectory() => Path.Combine(Directory.GetCurrentDirectory(), "sqllog.txt")
                using StreamWriter writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "sqllog.txt"), append: true);//append true có nghĩa là ghi đè
                writer.WriteLine(command.CommandText);//command.CommnadText chính là cau sql của ban
            }
            return base.ReaderExecuted(command, eventData, result);
        }
    }
}
