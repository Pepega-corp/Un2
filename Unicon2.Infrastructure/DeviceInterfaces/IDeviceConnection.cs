using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    public interface IDeviceConnection : ICloneable, IDisposable
    {
        string ConnectionName { get; }
        Task<Result> TryOpenConnectionAsync(IDeviceLogger currentDeviceLogger);
        void CloseConnection();
    }
}