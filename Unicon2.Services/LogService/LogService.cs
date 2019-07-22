using System;
using System.Collections.Generic;
using NLog;
using NLog.Targets;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Services.LogService
{
    public class LogService : ILogService
    {
        private readonly Func<ILogMessage> _logMessageGettingFunc;
        private readonly ILocalizerService _localizerService;
        private List<IDeviceLogger> _deviceLoggers = new List<IDeviceLogger>();
        private List<ILogMessage> _logMessages = new List<ILogMessage>();
        private string _filePath;

        public LogService(Func<ILogMessage> logMessageGettingFunc, ILocalizerService localizerService)
        {
            _logMessageGettingFunc = logMessageGettingFunc;
            _localizerService = localizerService;
        }

        #region Implementation of ILogService

        public bool IsLoggingToFileEnabled { get; set; }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                SetFilePath();
            }
        }

        private void SetFilePath()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = FilePath };

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config;


            //FileTarget target = (FileTarget)LogManager.Configuration.FindTargetByName("file");
            //if (target == null)
            //{
            //    target = new FileTarget(FilePath);
            //    LogManager.Configuration.AddTarget("logfile", target);
            //    LogManager.Configuration.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, target));
            //}

            //target.FileName = _filePath;
            LogManager.ReconfigExistingLoggers();
        }

        public void AddLogger(IDeviceLogger logger, string subjectName)
        {
            logger.SetLoggerSubject(subjectName);
            if (_deviceLoggers.Contains(logger))
            {
                return;
            }
            _deviceLoggers.Add(logger);

            logger.LogMessageAriseAction += OnLogMessageArise;
            LoggersChangedAction?.Invoke();
        }

        private void OnLogMessageArise(ILogMessage logMessage)
        {
            _logMessages.Add(logMessage);
            if (IsLoggingToFileEnabled)
            {
                var logger = LogManager.GetLogger(logMessage.MessageSubject);
                logger.Trace(logMessage.ToString);
            }
            NewMessageAction?.Invoke(logMessage);
        }

        public void DeleteLogger(IDeviceLogger logger)
        {
            _deviceLoggers.Remove(logger);
            if (logger?.LogMessageAriseAction != null)
            {
                logger.LogMessageAriseAction -= OnLogMessageArise;
            }
            LoggersChangedAction?.Invoke();

        }

        public Action<ILogMessage> NewMessageAction { get; set; }

        public Action LoggersChangedAction { get; set; }
        public IEnumerable<IDeviceLogger> GetLoggersEnumerable()
        {
            return _deviceLoggers;
        }

        public void RaiseInfoMessage(string messageKey)
        {
            ILogMessage logMessage = _logMessageGettingFunc();
            logMessage.LogMessageType = LogMessageTypeEnum.Info;
            logMessage.MessageSubject =_localizerService.GetLocalizedString(ApplicationGlobalNames.GENERAL);
            string localizedString = String.Empty;
            if (!_localizerService.TryGetLocalizedString(messageKey, out localizedString))
            {
                localizedString = messageKey;
            }
            logMessage.Description = localizedString;
            logMessage.MessageDateTime = DateTime.Now;
            NewMessageAction?.Invoke(logMessage);
        }

        #endregion
    }
}