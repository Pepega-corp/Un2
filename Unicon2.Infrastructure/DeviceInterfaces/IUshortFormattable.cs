using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IUshortFormattable : INameable, ICloneable
    {
        IUshortsFormatter UshortsFormatter { get; set; }
    }

    public interface IBitConfigurable
    {
        bool IsFromBits { get; set; }
        List<ushort> BitNumbers { get; set; }
    }
}