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
            this._model = modbusTcpConnection;
            this.PingDeviceCommand = new RelayCommand(this.OnPingDeviceCommand);
        }



        #region Implementation of IModbusTcpConnectionViewModel

        public int Port
        {
            get { return this._port; }
            set
            {
                this._port = value;
                this.RaisePropertyChanged();
            }
        }

        public string IpAddress
        {
            get { return this._ipAddress; }
            set
            {
                this._ipAddress = value;
                this.RaisePropertyChanged();
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
        public string ConnectionName => this._model.ConnectionName;

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => ModBusTcpKeys.MODBUS_TCP_CONNECTION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {
                this._model.IpAddress = this.IpAddress;
                this._model.Port = this.Port;
                return this._model;
            }
            set
            {
                this._model = value as IModbusTcpConnection;
                this.Port = this._model.Port;
                this.IpAddress = this._model.IpAddress;
            }
        }

        #endregion
    }
}
