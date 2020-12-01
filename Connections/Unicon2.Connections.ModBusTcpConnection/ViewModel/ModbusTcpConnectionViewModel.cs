using Unicon2.Connections.ModBusTcpConnection.Interfaces;
using Unicon2.Connections.ModBusTcpConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Unicon2.Unity.Commands;

namespace Unicon2.Connections.ModBusTcpConnection.ViewModel
{
    public class ModbusTcpConnectionViewModel : ViewModelBase, IModbusTcpConnectionViewModel, IDeviceConnectionViewModel
    {
        private int _port;
        private IModbusTcpConnection _model;
        private string _ipAddress;
        private bool _isPinging;

        public ICommand PingDeviceCommand { get; set; }


        public ModbusTcpConnectionViewModel(IModbusTcpConnection modbusTcpConnection)
        {
            _model = modbusTcpConnection;
            PingDeviceCommand = new RelayCommand(OnPingDeviceCommand);
        }


        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                RaisePropertyChanged();
            }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPinging
        {
            get { return _isPinging; }
            set
            {
                _isPinging = value;
                RaisePropertyChanged();
            }
        }

        private void OnPingDeviceCommand()
        {

            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(IpAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            IsPinging = pingable;

        }
        public string ConnectionName => _model.ConnectionName;

        public string StrongName => ModBusTcpKeys.MODBUS_TCP_CONNECTION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get
            {
                _model.IpAddress = IpAddress;
                _model.Port = Port;
                return _model;
            }
            set
            {
                _model = value as IModbusTcpConnection;
                Port = _model.Port;
                IpAddress = _model.IpAddress;
            }
        }
    }
}
