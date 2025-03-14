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
            using StreamWriter writer = new StreamWriter("D:\\NetProject\\TodoWeb\\TodoWeb\\sqllog.txt", append: true);//append true có nghĩa là ghi đè
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
                using StreamWriter writer = new StreamWriter("D:\\NetProject\\TodoWeb\\TodoWeb\\sqllog.txt", append: true);//append true có nghĩa là ghi đè
                writer.WriteLine(command.CommandText);//command.CommnadText chính là cau sql của ban
            }
            return base.ReaderExecuted(command, eventData, result);
        }
    }
}
