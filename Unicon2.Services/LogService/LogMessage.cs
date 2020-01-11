using System;
using System.Globalization;
using System.Text;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Services.LogService
{
    public class LogMessage : ILogMessage
    {
        public string MessageSubject { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string Description { get; set; }
        public LogMessageTypeEnum LogMessageType { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageDateTime.ToString(new CultureInfo("de-DE")));
            sb.Append(Description);
            return sb.ToString();
        }
    }
}