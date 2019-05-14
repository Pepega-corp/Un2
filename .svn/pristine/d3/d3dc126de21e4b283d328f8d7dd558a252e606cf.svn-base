using System;
using System.Collections.Generic;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels
{
    public interface IModbusMemoryEntityViewModel:ICloneable
    {
        void SetAddress(int address);
        void SetViewSetOption(bool isDiscret);
        bool IsDiscretView { get; }
        IModbusConversionParametersViewModel ModbusConversionParametersViewModel { get; set; }
        string AdressHex { get;  }
        string AdressDec { get; }
        List<IMemoryBitViewModel> Bits { get; }
        string ConvertedValue { get;  }
        string DirectValueHex { get; }
        string DirectValueDec { get; }

        void SetUshortValue(ushort value);
        void SetError();
        bool IsError { get; }
    }
}