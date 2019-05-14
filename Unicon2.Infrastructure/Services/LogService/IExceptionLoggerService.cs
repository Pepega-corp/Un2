using System;

namespace Unicon2.Infrastructure.Services.LogService
{
    public interface IExceptionLoggerService
    {
        void LogExceptionInFile(Exception exception);
    }
}