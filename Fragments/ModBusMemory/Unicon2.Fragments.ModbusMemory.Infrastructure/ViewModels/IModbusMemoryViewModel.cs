using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels
{
    public interface IModbusMemoryViewModel : IFragmentViewModel, IDisposable, IDeviceContextConsumer, IStronglyNamed
    {
        ObservableCollection<IModbusMemoryEntityViewModel> ModbusMemoryEntityViewModels { get; set; }
        IModbusMemorySettingsViewModel ModbusMemorySettingsViewModel { get; set; }
        bool IsQueriesStarted { get; set; }
        ICommand ExecuteOneQueryCommand { get; }
        ICommand EditEntityCommand { get; }
        ICommand SetTrueBitCommand { get; }
        ICommand SetFalseBitCommand { get; }

    }

}