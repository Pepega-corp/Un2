using Unicon2.Connections.ModBusTcpConnection.Interfaces;
using Unicon2.Connections.ModBusTcpConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.ModBusTcpConnection.ViewModel
{
    public class ModbusTcpConnectionViewModel : ViewModelBase, IModbusTcpConnectionViewModel
    {
        private int _port;
        private IModbusTcpConnection _model;
        private string _ipAddress;


        public ModbusTcpConnectionViewModel(IModbusTcpConnection modbusTcpConnection)
        {
            this._model = modbusTcpConnection;
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
