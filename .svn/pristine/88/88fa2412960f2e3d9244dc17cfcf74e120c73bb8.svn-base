using System;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Services.LogService
{
    public interface IDeviceLogger:ICloneable,IInitializableFromContainer
    {
        void LogInfoMessage(string description);
        void LogError(string description);
        void LogSuccessfulQuery(string description);
        void LogFailedQuery(string description);
        void SetLoggerSubject(string subjectName);
        Action<ILogMessage> LogMessageAriseAction { get; set; }
        bool IsInfoMessagesLoggingEnabled { get; set; }
        bool IsFailedQueriesLoggingEnabled { get; set; }
        bool IsSuccessfulQueriesLoggingEnabled { get; set; }

        bool IsErrorsLoggingEnabled { get; set; }






        string SourceName { get; set; }
    }
}