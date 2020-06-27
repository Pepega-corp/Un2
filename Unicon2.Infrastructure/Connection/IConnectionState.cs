using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Infrastructure.Connection
{
    public interface IConnectionState : ICloneable
    {
        string RelatedResourceString { get; set; }
        List<string> ExpectedValues { get; set; }
        IComPortConfiguration DefaultComPortConfiguration { get; set; }
    }
}