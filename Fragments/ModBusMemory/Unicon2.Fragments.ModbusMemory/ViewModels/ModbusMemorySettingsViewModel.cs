using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.ModbusMemory.ViewModels
{
    public class ModbusMemorySettingsViewModel : ValidatableBindableBase, IModbusMemorySettingsViewModel
    {
        private int _baseAdress;
        private int _numberOfPoints;
        private IModbusMemorySettings _modbusMemorySettings;
        private ObservableCollection<int> _numberOfPointsCollection;
        private bool _isDiscretTabSelected;

        public ModbusMemorySettingsViewModel(IModbusMemorySettings modbusMemorySettings)
        {
            this._modbusMemorySettings = modbusMemorySettings;
            this.NumberOfPointsCollection = new ObservableCollection<int>() { 8, 16, 24, 32 };
            this.NumberOfPoints = this.NumberOfPointsCollection[1];
            this.AddressStepDownCommand = new RelayCommand(this.OnAddressStepDownExecute);
            this.AddressStepUpCommand = new RelayCommand(this.OnAddressStepUpExecute);
            this.IsDiscretTabSelected = false;
        }

        private void OnAddressStepUpExecute()
        {
            this._baseAdress += this.NumberOfPoints;
            this.RaisePropertyChanged(nameof(this.BaseAdressHex));
            this.RaisePropertyChanged(nameof(this.BaseAdressDec));
            this.ModbusMemorySettingsChanged?.Invoke(this.GetModbusMemorySettings());

        }

        private void OnAddressStepDownExecute()
        {
            if (this._baseAdress - this.NumberOfPoints < 0)
            {
                this._baseAdress = 0;
            }
            else
            {
                this._baseAdress -= this.NumberOfPoints;
            }
            this.RaisePropertyChanged(nameof(this.BaseAdressHex));
            this.RaisePropertyChanged(nameof(this.BaseAdressDec));
            this.ModbusMemorySettingsChanged?.Invoke(this.GetModbusMemorySettings());

        }

        protected override void OnValidate()
        {
            throw new NotImplementedException();
        }

        public ICommand AddressStepDownCommand { get; set; }

        public bool IsDiscretTabSelected
        {
            get { return this._isDiscretTabSelected; }
            set
            {
                this._isDiscretTabSelected = value;
                this.ModbusMemorySettingsChanged?.Invoke(this.GetModbusMemorySettings());
                this.RaisePropertyChanged();
            }
        }

        public string BaseAdressDec
        {
            get { return this._baseAdress.ToString(); }
            set
            { this._baseAdress = int.Parse(value);
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.BaseAdressHex));
                this.ModbusMemorySettingsChanged?.Invoke(this.GetModbusMemorySettings());
            }
        }

        public string BaseAdressHex
        {
            get { return this._baseAdress.ToString("X"); }
            set
            {
                this._baseAdress = int.Parse(value, NumberStyles.HexNumber);
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.BaseAdressDec));
                this.ModbusMemorySettingsChanged?.Invoke(this.GetModbusMemorySettings());
            }
        }

        public int NumberOfPoints
        {
            get { return this._numberOfPoints; }
            set
            {
                this._numberOfPoints = value;
                this.RaisePropertyChanged();
                this.ModbusMemorySettingsChanged?.Invoke(this.GetModbusMemorySettings());

            }
        }


        public ObservableCollection<int> NumberOfPointsCollection
        {
            get { return this._numberOfPointsCollection; }
            set
            {
                this._numberOfPointsCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public Action<IModbusMemorySettings> ModbusMemorySettingsChanged { get; set; }

        public IModbusMemorySettings GetModbusMemorySettings()
        {
            this._modbusMemorySettings.BaseAdress = this._baseAdress;
            this._modbusMemorySettings.NumberOfPoints = this.NumberOfPoints;
            this._modbusMemorySettings.IsDiscretOption = this._isDiscretTabSelected;
            return this._modbusMemorySettings;
        }

        public ICommand AddressStepUpCommand { get; set; }
    }
}
