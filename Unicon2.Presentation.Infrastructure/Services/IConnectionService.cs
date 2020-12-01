using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IConnectionService
    {
        Task<Result<string>> CheckConnection(IConnectionState connectionState, DeviceContext.DeviceContext deviceContext);
    }
}