using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Infrastructure.Connection
{
    public interface IConnectionState : ICloneable
    {
        string RelatedResourceString { get; set; }
        List<string> ExpectedValues { get; set; }
        IComPortConfiguration DefaultComPortConfiguration { get; set; }
    }
}