using System;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Services.LogService
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceLogger : IDeviceLogger
    {

        public DeviceLogger()
        {
            IsInfoMessagesLoggingEnabled = true;
            IsErrorsLoggingEnabled = true;
            IsFailedQueriesLoggingEnabled = true;
        }


        public void LogInfoMessage(string description)
        {
            if (!IsInfoMessagesLoggingEnabled) return;
            ILogMessage logMessage = GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.Info;
            LogMessageAriseAction?.Invoke(logMessage);
        }

        public void LogError(string description)
        {
            if(!IsErrorsLoggingEnabled)return;
            ILogMessage logMessage = GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.Error;
            LogMessageAriseAction?.Invoke(logMessage);
        }

        public void LogSuccessfulQuery(string description)
        {
            if (!IsSuccessfulQueriesLoggingEnabled) return;
            ILogMessage logMessage = GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.SuccsessfulQuery;
            LogMessageAriseAction?.Invoke(logMessage);
        }

        public void LogFailedQuery(string description)
        {
            if (!IsFailedQueriesLoggingEnabled) return;
            ILogMessage logMessage = GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.FailedQuery;
            LogMessageAriseAction?.Invoke(logMessage);

        }

        public void SetLoggerSubject(string subjectName)
        {
            SourceName = subjectName;
        }


        private ILogMessage GetRawMessage()
        {
            ILogMessage logMessageRaw = StaticContainer.Container.Resolve<ILogMessage>();
            logMessageRaw.MessageSubject = SourceName;
            logMessageRaw.MessageDateTime = DateTime.Now;
            return logMessageRaw;
        }

        public Action<ILogMessage> LogMessageAriseAction { get; set; }

        [JsonProperty]
        public bool IsInfoMessagesLoggingEnabled { get; set; }

        [JsonProperty]
        public bool IsFailedQueriesLoggingEnabled { get; set; }

        [JsonProperty]
        public bool IsSuccessfulQueriesLoggingEnabled { get; set; }

        [JsonProperty]
        public bool IsErrorsLoggingEnabled { get; set; }

        [JsonProperty]
        public string SourceName { get; set; }

        public object Clone()
        {
            IDeviceLogger deviceLogger=new DeviceLogger();
           deviceLogger.SetLoggerSubject(SourceName);
            return deviceLogger;
        }
    }
}