using System;
using System.Collections.Generic;

namespace Unicon2.Infrastructure.Services.LogService
{
    public interface ILogService
    {
        bool IsLoggingToFileEnabled { get; set; }
        string FilePath { get; set; }
        void AddLogger(IDeviceLogger logger, string subjectName);
        void DeleteLogger(IDeviceLogger logger);
        Action<ILogMessage> NewMessageAction { get; set; }
        Action LoggersChangedAction { get; set; }
        IEnumerable<IDeviceLogger> GetLoggersEnumerable();
        void LogMessage(string messageKey, LogMessageTypeEnum messageType = LogMessageTypeEnum.Info);
    }
}