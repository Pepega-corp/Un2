using System;

namespace Unicon2.Infrastructure.Interfaces.Values
{
    public interface ILocalAndDeviceValuesContaining : IDeviceValueContaining
    {
        ushort[] LocalUshortsValue { get; set; }
        Action LocalUshortsValueChanged { get; set; }
        bool IsValuesEqual { get; }
    }
}