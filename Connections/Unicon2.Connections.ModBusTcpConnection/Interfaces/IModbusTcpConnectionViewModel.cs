using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Connections.ModBusTcpConnection.Interfaces
{
    public interface IModbusTcpConnectionViewModel : IViewModel
    {
        int Port { get; set; }
        string IpAddress { get; set; }
        string ConnectionName { get; }
    }
}