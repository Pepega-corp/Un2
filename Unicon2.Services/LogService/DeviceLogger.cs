using System;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Services.LogService
{
    [DataContract(Namespace = "DeviceLoggerNS")]
    public class DeviceLogger : IDeviceLogger
    {
        private Func<ILogMessage> _logMessageGettingFunc;

        public DeviceLogger(Func<ILogMessage> logMessageGettingFunc)
        {
            this._logMessageGettingFunc = logMessageGettingFunc;
            this.IsInfoMessagesLoggingEnabled = true;
            this.IsErrorsLoggingEnabled = true;
            this.IsFailedQueriesLoggingEnabled = true;
        }


        public void LogInfoMessage(string description)
        {
            if (!this.IsInfoMessagesLoggingEnabled) return;
            ILogMessage logMessage = this.GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.Info;
            this.LogMessageAriseAction?.Invoke(logMessage);
        }

        public void LogError(string description)
        {
            if(!this.IsErrorsLoggingEnabled)return;
            ILogMessage logMessage = this.GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.Error;
            this.LogMessageAriseAction?.Invoke(logMessage);
        }

        public void LogSuccessfulQuery(string description)
        {
            if (!this.IsSuccessfulQueriesLoggingEnabled) return;
            ILogMessage logMessage = this.GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.SuccsessfulQuery;
            this.LogMessageAriseAction?.Invoke(logMessage);
        }

        public void LogFailedQuery(string description)
        {
            if (!this.IsFailedQueriesLoggingEnabled) return;
            ILogMessage logMessage = this.GetRawMessage();
            logMessage.Description = description;
            logMessage.LogMessageType = LogMessageTypeEnum.FailedQuery;
            this.LogMessageAriseAction?.Invoke(logMessage);

        }

        public void SetLoggerSubject(string subjectName)
        {
            this.SourceName = subjectName;
        }


        private ILogMessage GetRawMessage()
        {
            ILogMessage logMessageRaw = this._logMessageGettingFunc();
            logMessageRaw.MessageSubject = this.SourceName;
            logMessageRaw.MessageDateTime = DateTime.Now;
            return logMessageRaw;
        }

        public Action<ILogMessage> LogMessageAriseAction { get; set; }

        [DataMember]
        public bool IsInfoMessagesLoggingEnabled { get; set; }

        [DataMember]
        public bool IsFailedQueriesLoggingEnabled { get; set; }

        [DataMember]
        public bool IsSuccessfulQueriesLoggingEnabled { get; set; }

        [DataMember]
        public bool IsErrorsLoggingEnabled { get; set; }

        [DataMember]
        public string SourceName { get; set; }

        public object Clone()
        {
            IDeviceLogger deviceLogger=new DeviceLogger(this._logMessageGettingFunc);
           deviceLogger.SetLoggerSubject(this.SourceName);
            return deviceLogger;
        }

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (this.IsInitialized) return;

            this._logMessageGettingFunc = container.Resolve<Func<ILogMessage>>();
            this.IsInitialized = true;
        }
    }
}