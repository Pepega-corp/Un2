using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels
{
    public interface IModbusMemorySettingsViewModel
    {
        IModbusMemorySettings GetModbusMemorySettings();
        ICommand AddressStepUpCommand { get; set; }
        ICommand AddressStepDownCommand { get; set; }
        bool IsDiscretTabSelected { get; set; }
        string BaseAdressDec { get; set; }
        string BaseAdressHex { get; set; }
        int NumberOfPoints { get; set; }
        ObservableCollection<int> NumberOfPointsCollection { get; set; }
        Action<IModbusMemorySettings> ModbusMemorySettingsChanged { get; set; }
    }
}