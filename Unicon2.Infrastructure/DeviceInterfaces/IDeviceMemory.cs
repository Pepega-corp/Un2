﻿using System.Collections.Generic;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IDeviceMemory
    {
        Dictionary<ushort, ushort> DeviceMemoryValues { get; set; }
        Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
        Dictionary<ushort, bool> DeviceBitMemoryValues { get; set; }
        Dictionary<ushort, bool> LocalBitMemoryValues { get; set; }
    }
}