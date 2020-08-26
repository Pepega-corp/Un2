using System;
using System.IO;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Services.LogService
{
    public class ExceptionLoggerService : IExceptionLoggerService
    {
        public void LogExceptionInFile(Exception exception)
        {
            try
            {

                using (StreamWriter writer = new StreamWriter(new FileStream("ErrorLog.txt", FileMode.Append | FileMode.OpenOrCreate)))
                {
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine($"[{DateTime.Now.ToString()}] Exception");
                    writer.WriteLine(exception.Message);
                    writer.WriteLine(exception.StackTrace);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
