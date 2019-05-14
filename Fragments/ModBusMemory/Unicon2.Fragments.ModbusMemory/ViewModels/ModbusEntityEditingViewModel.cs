using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Fragments.ModbusMemory.ViewModels.Validators;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.ModbusMemory.ViewModels
{
    public class ModbusEntityEditingViewModel : ValidatableBindableBase, IModbusEntityEditingViewModel
    {
        private readonly ILocalizerService _localizerService;
        private IDataProvider _dataProvider;
        private ushort _resultedValueUshort;
        private string _valueDec;
        private string _valueHex;

        public ModbusEntityEditingViewModel(ILocalizerService localizerService)
        {
            this._localizerService = localizerService;
            this.WriteCommand = new RelayCommand<object>(this.OnExecuteWrite);
            this.CancelCommand = new RelayCommand<object>(this.OnExecuteCancel);
            this.ChangeBitValueCommand = new RelayCommand<int?>(this.OnExecuteChangeBitValue);
        }

        private void OnExecuteChangeBitValue(int? bitNumber)
        {
            if (!bitNumber.HasValue) throw new ArgumentException();
            bool[] bools = new bool[16];
            foreach (IMemoryBitViewModel memoryBitViewModel in this.ModbusMemoryEntityViewModelToEdit.Bits)
            {
                bools[memoryBitViewModel.BitNumber] = memoryBitViewModel.BoolValue.Value;
            }
            bools[bitNumber.Value] = !bools[bitNumber.Value];
            ushort resultedValue = 0;
            for (int i = 0; i < bools.Count(); i++)
            {
                if (bools[i])
                {
                    this._resultedValueUshort = resultedValue += (ushort)(1 << i);
                }
            }
            this.ModbusMemoryEntityViewModelToEdit.SetUshortValue(resultedValue);
            this.ValueDec = this.ModbusMemoryEntityViewModelToEdit.DirectValueDec;
            this.ValueHex = this.ModbusMemoryEntityViewModelToEdit.DirectValueHex;
        }

        private void OnExecuteCancel(object obj)
        {
            if (obj is Window)
            {
                ((Window)obj).Close();
            }
        }

        private void OnExecuteWrite(object obj)
        {
            this._dataProvider.WriteMultipleRegistersAsync(ushort.Parse(this.ModbusMemoryEntityViewModelToEdit.AdressDec), new[] { this._resultedValueUshort }, ApplicationGlobalNames.QueriesNames.WRITE_MODBUS_MEMORY_QUERY_KEY);
            if (obj is Window)
            {
                ((Window)obj).Close();
            }
        }


        #region Implementation of IModbusEntityEditingViewModel

        public void SetEntity(IModbusMemoryEntityViewModel modbusMemoryEntityViewModelToEdit)
        {
            this.ModbusMemoryEntityViewModelToEdit = modbusMemoryEntityViewModelToEdit;
            this.RaisePropertyChanged(nameof(this.ModbusMemoryEntityViewModelToEdit));
            this.ValueDec = this.ModbusMemoryEntityViewModelToEdit.DirectValueDec;
            this.ValueHex = this.ModbusMemoryEntityViewModelToEdit.DirectValueHex;
            this.RaisePropertyChanged(nameof(this.ModbusMemoryEntityViewModelToEdit.AdressDec));
            this.RaisePropertyChanged(nameof(this.ModbusMemoryEntityViewModelToEdit.AdressHex));
            this._resultedValueUshort = ushort.Parse(this.ModbusMemoryEntityViewModelToEdit.DirectValueDec);
        }

        public IModbusMemoryEntityViewModel ModbusMemoryEntityViewModelToEdit { get; private set; }

        public ICommand WriteCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand ChangeBitValueCommand { get; }

        public string ValueHex
        {
            get { return this._valueHex; }
            set
            {
                this._valueHex = value;
                this.FireErrorsChanged(nameof(this.ValueHex));
                if (this.HasErrors)
                {
                    this._valueHex = this.ModbusMemoryEntityViewModelToEdit.DirectValueHex;
                    return;
                }
                this.ModbusMemoryEntityViewModelToEdit.SetUshortValue(Convert.ToUInt16(value, 16));
                this.RaisePropertyChanged();

                if (this.ValueDec != this.ModbusMemoryEntityViewModelToEdit.DirectValueDec)
                {
                    this.ValueDec = this.ModbusMemoryEntityViewModelToEdit.DirectValueDec;
                }

                this.RaisePropertyChanged(nameof(this.ValueDec));
                this._resultedValueUshort = ushort.Parse(this.ModbusMemoryEntityViewModelToEdit.DirectValueDec);
            }
        }

        public string ValueDec
        {
            get { return this._valueDec; }
            set
            {
                this._valueDec = value;
                this.FireErrorsChanged(nameof(this.ValueDec));
                if (this.HasErrors)
                {
                    if (this.HasErrors)
                    {
                        this._valueDec = this.ModbusMemoryEntityViewModelToEdit.DirectValueDec;
                        return;
                    }
                }
                this.ModbusMemoryEntityViewModelToEdit.SetUshortValue(Convert.ToUInt16(value));
                this.RaisePropertyChanged();


                if (this.ValueHex != this.ModbusMemoryEntityViewModelToEdit.DirectValueHex)
                {
                    this.ValueHex = this.ModbusMemoryEntityViewModelToEdit.DirectValueHex;
                }


                this.RaisePropertyChanged(nameof(this.ValueHex));
                this._resultedValueUshort = ushort.Parse(this.ModbusMemoryEntityViewModelToEdit.DirectValueDec);

            }
        }

        #endregion

        #region Implementation of IDataProviderContaining

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        #endregion

        #region Implementation of ICloneable



        #endregion

        #region Overrides of ValidatableBindableBase

        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult res = new ModbusEntityEditingViewModelValidator(this._localizerService).Validate(this);
            this.SetValidationErrors(res);

        }

        #endregion
    }
}
