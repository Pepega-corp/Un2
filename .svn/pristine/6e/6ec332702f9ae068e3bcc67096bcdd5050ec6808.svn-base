using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using Unicon2.Connections.ModBusRtuConnection.Enums;
using Unicon2.Connections.ModBusRtuConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels
{
    /// <summary>
    /// конфигурация ком-порта
    /// </summary>
    public class ComPortConfigurationViewModel : ViewModelBase, IComPortConfigurationViewModel
    {
        #region private fields

        private ObservableCollection<int> _baudRates;
        private int _selectedBaudRate;
        private ObservableCollection<int> _dataBitsCollection;
        private int _selectedDataBits;
        private ObservableCollection<StopBits> _stopBitsCollection;
        private StopBits _selectedStopBits;
        private ObservableCollection<Parity> _parityCollection;
        private Parity _selectedParity;
        private int _waitAnswer;
        private int _waitByte;
        private int _onTransmission;
        private int _offTramsmission;
        private IComPortConfiguration _comPortConfiguration;

        #endregion

        #region ctor
        public ComPortConfigurationViewModel(IComPortConfiguration comPortConfiguration)
        {
            this._comPortConfiguration = comPortConfiguration;
            this.BaudRates = new ObservableCollection<int>();
            foreach (BaudRatesEnum baudRatesEnum in Enum.GetValues(typeof(BaudRatesEnum)))
            {
                this.BaudRates.Add((int)baudRatesEnum);
            }
            this.DataBitsCollection = new ObservableCollection<int>();
            foreach (DataBitsEnum dataBit in Enum.GetValues(typeof(DataBitsEnum)))
            {
                this.DataBitsCollection.Add((int)dataBit);
            }

            this.StopBitsCollection = new ObservableCollection<StopBits>();
            foreach (StopBits stopBit in Enum.GetValues(typeof(StopBits)))
            {
                this.StopBitsCollection.Add(stopBit);
            }
            this.ParityCollection = new ObservableCollection<Parity>();
            foreach (Parity parity in Enum.GetValues(typeof(Parity)))
            {
                this.ParityCollection.Add(parity);
            }


            this.SelectedBaudRate = (int)BaudRatesEnum.BR115200;
            this.SelectedDataBits = 8;
            this.SelectedStopBits = StopBits.One;
            this.SelectedParity = Parity.None;
            this.WaitAnswer = 200;
            this.WaitByte = 100;
            this.OnTransmission = 0;
            this.OffTramsmission = 0;

        }
        #endregion

        #region Presentation Properties




        public int SelectedBaudRate
        {
            get => this._selectedBaudRate;
            set
            {
                this._selectedBaudRate = value;
                this.RaisePropertyChanged();
            }
        }

        public int SelectedDataBits
        {
            get { return this._selectedDataBits; }
            set
            {
                this._selectedDataBits = value;
                this.RaisePropertyChanged();
            }
        }

        public StopBits SelectedStopBits
        {
            get { return this._selectedStopBits; }
            set
            {
                this._selectedStopBits = value;
                this.RaisePropertyChanged();
            }
        }

        public Parity SelectedParity
        {
            get { return this._selectedParity; }
            set
            {
                this._selectedParity = value;
                this.RaisePropertyChanged();
            }
        }

        public int WaitAnswer
        {
            get { return this._waitAnswer; }
            set
            {
                this._waitAnswer = value;
                this.RaisePropertyChanged();
            }
        }

        public int WaitByte
        {
            get { return this._waitByte; }
            set
            {
                this._waitByte = value;
                this.RaisePropertyChanged();
            }
        }

        public int OnTransmission
        {
            get { return this._onTransmission; }
            set
            {
                this._onTransmission = value;
                this.RaisePropertyChanged();
            }
        }

        public int OffTramsmission
        {
            get { return this._offTramsmission; }
            set
            {
                this._offTramsmission = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> BaudRates
        {
            get { return this._baudRates; }
            set
            {
                this._baudRates = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> DataBitsCollection
        {
            get { return this._dataBitsCollection; }
            set
            {
                this._dataBitsCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<StopBits> StopBitsCollection
        {
            get { return this._stopBitsCollection; }
            set
            {
                this._stopBitsCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<Parity> ParityCollection
        {
            get { return this._parityCollection; }
            set
            {
                this._parityCollection = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => StringKeys.COMPORT_CONFIGURATION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {

                this._comPortConfiguration.Parity = this.SelectedParity;
                this._comPortConfiguration.BaudRate = this.SelectedBaudRate;
                this._comPortConfiguration.DataBits = this.SelectedDataBits;
                this._comPortConfiguration.OffTramsmission = this.OffTramsmission;
                this._comPortConfiguration.OnTransmission = this.OnTransmission;
                this._comPortConfiguration.StopBits = this.SelectedStopBits;
                this._comPortConfiguration.WaitAnswer = this.WaitAnswer;
                this._comPortConfiguration.WaitByte = this.WaitByte;
                return this._comPortConfiguration;
            }
            set
            {
                if (!(value is IComPortConfiguration)) return;
                this._comPortConfiguration = value as IComPortConfiguration;
                this.SelectedParity = this._comPortConfiguration.Parity;
                this.SelectedBaudRate = this._comPortConfiguration.BaudRate;
                this.SelectedDataBits = this._comPortConfiguration.DataBits;
                this.OffTramsmission = this._comPortConfiguration.OffTramsmission;
                this.OnTransmission = this._comPortConfiguration.OnTransmission;
                this.SelectedStopBits = this._comPortConfiguration.StopBits;
                this.WaitAnswer = this._comPortConfiguration.WaitAnswer;
                this.WaitByte = this._comPortConfiguration.WaitByte;
            }
        }

        #endregion
    }
}