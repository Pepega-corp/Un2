using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Infrastructure.DeviceInterfaces.SharedResources
{
    public interface IDeviceSharedResources : IDisposable
    {
        List<INameable> SharedResources { get; set; }
    }
}