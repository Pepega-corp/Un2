using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using Unicon2.Connections.ModBusRtuConnection.Enums;
using Unicon2.Connections.ModBusRtuConnection.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels
{
    /// <summary>
    /// конфигурация ком-порта
    /// </summary>
    public class ComPortConfigurationViewModel : ValidatableBindableBase, IComPortConfigurationViewModel
    {
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

        public ComPortConfigurationViewModel(IComPortConfiguration comPortConfiguration)
        {
            _comPortConfiguration = comPortConfiguration;
            BaudRates = new ObservableCollection<int>();
            foreach (BaudRatesEnum baudRatesEnum in Enum.GetValues(typeof(BaudRatesEnum)))
            {
                BaudRates.Add((int)baudRatesEnum);
            }
            DataBitsCollection = new ObservableCollection<int>();
            foreach (DataBitsEnum dataBit in Enum.GetValues(typeof(DataBitsEnum)))
            {
                DataBitsCollection.Add((int)dataBit);
            }

            StopBitsCollection = new ObservableCollection<StopBits>();
            foreach (StopBits stopBit in Enum.GetValues(typeof(StopBits)))
            {
                StopBitsCollection.Add(stopBit);
            }
            ParityCollection = new ObservableCollection<Parity>();
            foreach (Parity parity in Enum.GetValues(typeof(Parity)))
            {
                ParityCollection.Add(parity);
            }


            SelectedBaudRate = (int)BaudRatesEnum.BR115200;
            SelectedDataBits = 8;
            SelectedStopBits = StopBits.One;
            SelectedParity = Parity.None;
            WaitAnswer = 200;
            WaitByte = 100;
            OnTransmission = 0;
            OffTramsmission = 0;

        }


        public int SelectedBaudRate
        {
            get => _selectedBaudRate;
            set
            {
                _selectedBaudRate = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedDataBits
        {
            get { return _selectedDataBits; }
            set
            {
                _selectedDataBits = value;
                RaisePropertyChanged();
            }
        }

        public StopBits SelectedStopBits
        {
            get { return _selectedStopBits; }
            set
            {
                _selectedStopBits = value;
                RaisePropertyChanged();
            }
        }

        public Parity SelectedParity
        {
            get { return _selectedParity; }
            set
            {
                _selectedParity = value;
                RaisePropertyChanged();
            }
        }

        public int WaitAnswer
        {
            get { return _waitAnswer; }
            set
            {
                _waitAnswer = value;
                RaisePropertyChanged();
            }
        }

        public int WaitByte
        {
            get { return _waitByte; }
            set
            {
                _waitByte = value;
                RaisePropertyChanged();
            }
        }

        public int OnTransmission
        {
            get { return _onTransmission; }
            set
            {
                _onTransmission = value;
                RaisePropertyChanged();
            }
        }

        public int OffTramsmission
        {
            get { return _offTramsmission; }
            set
            {
                _offTramsmission = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> BaudRates
        {
            get { return _baudRates; }
            set
            {
                _baudRates = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> DataBitsCollection
        {
            get { return _dataBitsCollection; }
            set
            {
                _dataBitsCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StopBits> StopBitsCollection
        {
            get { return _stopBitsCollection; }
            set
            {
                _stopBitsCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Parity> ParityCollection
        {
            get { return _parityCollection; }
            set
            {
                _parityCollection = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName => StringKeys.COMPORT_CONFIGURATION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get
            {

                _comPortConfiguration.Parity = SelectedParity;
                _comPortConfiguration.BaudRate = SelectedBaudRate;
                _comPortConfiguration.DataBits = SelectedDataBits;
                _comPortConfiguration.OffTramsmission = OffTramsmission;
                _comPortConfiguration.OnTransmission = OnTransmission;
                _comPortConfiguration.StopBits = SelectedStopBits;
                _comPortConfiguration.WaitAnswer = WaitAnswer;
                _comPortConfiguration.WaitByte = WaitByte;
                return _comPortConfiguration;
            }
            set
            {
                if (!(value is IComPortConfiguration)) return;
                _comPortConfiguration = value as IComPortConfiguration;
                SelectedParity = _comPortConfiguration.Parity;
                SelectedBaudRate = _comPortConfiguration.BaudRate;
                SelectedDataBits = _comPortConfiguration.DataBits;
                OffTramsmission = _comPortConfiguration.OffTramsmission;
                OnTransmission = _comPortConfiguration.OnTransmission;
                SelectedStopBits = _comPortConfiguration.StopBits;
                WaitAnswer = _comPortConfiguration.WaitAnswer;
                WaitByte = _comPortConfiguration.WaitByte;
            }
        }
    }
}