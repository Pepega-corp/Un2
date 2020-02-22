using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IUshortFormattable : INameable
    {
        IUshortsFormatter UshortsFormatter { get; set; }
    }
}