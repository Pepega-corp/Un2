using System;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IUshortFormattable : INameable, ICloneable
    {
        IUshortsFormatter UshortsFormatter { get; set; }
    }
}