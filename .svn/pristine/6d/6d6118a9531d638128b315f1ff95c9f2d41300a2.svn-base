using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels
{
    public interface IModbusEntityEditingViewModel:IDataProviderContaining
    {
        void SetEntity(IModbusMemoryEntityViewModel modbusMemoryEntityViewModelToEdit);
        IModbusMemoryEntityViewModel ModbusMemoryEntityViewModelToEdit { get; }
        ICommand WriteCommand { get; }
        ICommand CancelCommand { get; }
        ICommand ChangeBitValueCommand { get; }

      string ValueHex { get; set; }
        string ValueDec { get; set; }

    }

}