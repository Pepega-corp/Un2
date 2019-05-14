using System;

namespace Unicon2.Infrastructure.Services.LogService
{
    public interface ILogMessage
    {
        string MessageSubject { get; set; }
        DateTime MessageDateTime { get; set; }
        string Description { get; set; }
        LogMessageTypeEnum LogMessageType { get; set; }
        
    }
}