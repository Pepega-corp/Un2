using System;
using System.Collections.Generic;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.ModbusMemory.ViewModels
{
    public class ModbusMemoryEntityViewModel : ValidatableBindableBase, IModbusMemoryEntityViewModel
    {
        private IModbusMemoryEntity _modbusMemoryEntity;
        private readonly Func<IMemoryBitViewModel> _memoryBitGettingFunc;
        private readonly Func<IModbusMemoryEntity> _modbusMemoryEntityGettingFunc;
        private IModbusConversionParametersViewModel _modbusConversionParametersViewModel;
        private int _address;
        private List<IMemoryBitViewModel> _memoryBitViewModels;

        public ModbusMemoryEntityViewModel(Func<IMemoryBitViewModel> memoryBitGettingFunc, Func<IModbusMemoryEntity> modbusMemoryEntityGettingFunc,
            IModbusConversionParametersViewModel modbusConversionParametersViewModel)
        {
            this._modbusMemoryEntity = modbusMemoryEntityGettingFunc();
            this._memoryBitGettingFunc = memoryBitGettingFunc;
            this._modbusMemoryEntityGettingFunc = modbusMemoryEntityGettingFunc;
            this.IsError = true;
            this.ModbusConversionParametersViewModel = modbusConversionParametersViewModel;
            this._memoryBitViewModels = new List<IMemoryBitViewModel>(16);
            this._modbusMemoryEntity.SetConversion(this._modbusConversionParametersViewModel.GetConversionParameters());

        }

        private void OnMemoryConversionParametersChanged(IMemoryConversionParameters memoryConversionParameters)
        {
            this._modbusMemoryEntity.SetConversion(memoryConversionParameters);
            this.RaisePropertyChanged(nameof(this.ConvertedValue));

        }

        #region Implementation of IModbusMemoryEntityViewModel

        public void SetAddress(int address)
        {
            this._address = address;
            if (this.IsDiscretView)
            {
                this.AdressDec = this._address.ToString("D") + "-" + (this._address + 16).ToString("D");
                this.AdressHex = this._address.ToString("X") + "-" + (this._address + 16).ToString("X");

            }
            else
            {
                this.AdressDec = this._address.ToString("D");
                this.AdressHex = this._address.ToString("X");
            }
            this.RaisePropertyChanged(nameof(this.AdressDec));
            this.RaisePropertyChanged(nameof(this.AdressHex));

        }

        public void SetViewSetOption(bool isDiscret)
        {
            if (this.IsDiscretView != isDiscret)
            {
                this.IsDiscretView = isDiscret;
                this.SetAddress(this._address);
                return;
            }
            this.IsDiscretView = isDiscret;
            this.RaisePropertyChanged(nameof(this.IsDiscretView));
        }

        public bool IsDiscretView { get; private set; }


        public IModbusConversionParametersViewModel ModbusConversionParametersViewModel
        {
            get { return this._modbusConversionParametersViewModel; }
            set
            {
                try
                {
                    if (this._modbusConversionParametersViewModel != null)
                        if (this._modbusConversionParametersViewModel.MemoryConversionParametersChanged != null)
                        {
                            this._modbusConversionParametersViewModel.MemoryConversionParametersChanged = null;

                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                this._modbusConversionParametersViewModel = value;
                this._modbusConversionParametersViewModel.MemoryConversionParametersChanged +=
                    this.OnMemoryConversionParametersChanged;
                this.RaisePropertyChanged();
            }
        }

        public string AdressHex { get; private set; }

        public string AdressDec { get; private set; }


        public string ConvertedValue => this._modbusMemoryEntity.ConvertedValue;

        public string DirectValueHex => this._modbusMemoryEntity.DirectValue.ToString("X");

        public string DirectValueDec => this._modbusMemoryEntity.DirectValue.ToString("D");

        public void SetUshortValue(ushort value)
        {
            this.IsError = false;
            this._modbusMemoryEntity.SetUshortValue(value);
            this.RaisePropertyChanged(nameof(this.Bits));
            this.RaisePropertyChanged(nameof(this.ConvertedValue));
            this.RaisePropertyChanged(nameof(this.DirectValueHex));
            this.RaisePropertyChanged(nameof(this.DirectValueDec));
            this.RaisePropertyChanged(nameof(this.IsError));

        }

        public List<IMemoryBitViewModel> Bits
        {
            get
            {
                bool[] bitBools = new bool[16];
                //BitArray bitArray = new BitArray(new int[] {_modbusMemoryEntity.DirectValue});
                if (this.IsDiscretView)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        bitBools[i] = ((this._modbusMemoryEntity.DirectValue >> i) & 1) == 1;
                    }
                }
                else
                {
                    for (int i = 0; i < 16; i++)
                    {
                        bitBools[15 - i] = ((this._modbusMemoryEntity.DirectValue >> i) & 1) == 1;
                    }
                }


                for (int i = 0; i < 16; i++)
                {
                    if (this._memoryBitViewModels.Count > i)
                    {
                        this._memoryBitViewModels[i].BitNumber = 15 - i;
                        if (this.IsDiscretView)
                        {
                            this._memoryBitViewModels[i].SetAddressDec((ushort)(this._address + i));
                        }
                        if (this.IsError)
                        {
                            this._memoryBitViewModels[i].BoolValue = null;
                            this._memoryBitViewModels[i].IsError = true;

                        }
                        else
                        {
                            this._memoryBitViewModels[i].IsError = false;
                            this._memoryBitViewModels[i].BoolValue = bitBools[i];
                        }
                        continue;
                    }
                    IMemoryBitViewModel memoryBitViewModel = this._memoryBitGettingFunc();
                    memoryBitViewModel.BitNumber = 15 - i;
                    if (this.IsError)
                        memoryBitViewModel.BoolValue = null;
                    else
                        memoryBitViewModel.BoolValue = bitBools[i];


                    this._memoryBitViewModels.Add(memoryBitViewModel);
                }
                return this._memoryBitViewModels;
            }
        }

        public void SetError()
        {
            this.IsError = true;
            this._modbusMemoryEntity.SetUshortValue(0);
            this.RaisePropertyChanged(nameof(this.ConvertedValue));
            this.RaisePropertyChanged(nameof(this.DirectValueHex));
            this.RaisePropertyChanged(nameof(this.DirectValueDec));
            this.RaisePropertyChanged(nameof(this.Bits));
            this.RaisePropertyChanged(nameof(this.IsError));

        }

        public bool IsError { get; private set; }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            IModbusMemoryEntityViewModel memoryEntityViewModel = new ModbusMemoryEntityViewModel(this._memoryBitGettingFunc, this._modbusMemoryEntityGettingFunc, this._modbusConversionParametersViewModel);

            memoryEntityViewModel.SetUshortValue((ushort)this._modbusMemoryEntity.DirectValue);

            memoryEntityViewModel.SetAddress(this._address);

            return memoryEntityViewModel;

        }

        #endregion
    }
}