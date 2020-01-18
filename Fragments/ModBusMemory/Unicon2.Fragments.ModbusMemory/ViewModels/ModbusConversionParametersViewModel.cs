using System;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.ModbusMemory.ViewModels
{
    public class ModbusConversionParametersViewModel : ValidatableBindableBase, IModbusConversionParametersViewModel
    {
        private int _limitOfValue;
        private int _maximumOfUshortValue;
        private int _numberOfSigns;
        private readonly IMemoryConversionParameters _memoryConversionParameters;

        public ModbusConversionParametersViewModel(IMemoryConversionParameters memoryConversionParameters)
        {
            this._memoryConversionParameters = memoryConversionParameters;
            this._maximumOfUshortValue = this._memoryConversionParameters.MaximumOfUshortValue;
            this._numberOfSigns = this._memoryConversionParameters.NumberOfSigns;
            this._limitOfValue = this._memoryConversionParameters.LimitOfValue;
        }

        public int LimitOfValue
        {
            get { return this._limitOfValue; }
            set
            {
                this._limitOfValue = value;
                this.RaisePropertyChanged();
                this.MemoryConversionParametersChanged?.Invoke(this.GetConversionParameters());
            }
        }

        public int MaximumOfUshortValue
        {
            get { return this._maximumOfUshortValue; }
            set
            {
                this._maximumOfUshortValue = value;
                this.RaisePropertyChanged();
                this.MemoryConversionParametersChanged?.Invoke(this.GetConversionParameters());

            }
        }

        public int NumberOfSigns
        {
            get { return this._numberOfSigns; }
            set
            {
                this._numberOfSigns = value;
                this.RaisePropertyChanged();
                this.MemoryConversionParametersChanged?.Invoke(this.GetConversionParameters());

            }
        }


        public Action<IMemoryConversionParameters> MemoryConversionParametersChanged { get; set; }

        public IMemoryConversionParameters GetConversionParameters()
        {
            this._memoryConversionParameters.LimitOfValue = this.LimitOfValue;
            this._memoryConversionParameters.MaximumOfUshortValue = this.MaximumOfUshortValue;
            this._memoryConversionParameters.NumberOfSigns = this.NumberOfSigns;
            return this._memoryConversionParameters;
        }


    }
}
