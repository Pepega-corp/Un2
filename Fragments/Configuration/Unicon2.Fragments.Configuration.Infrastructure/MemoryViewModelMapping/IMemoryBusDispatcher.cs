using System;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping
{
    public interface IMemoryBusDispatcher : IDisposable
    {
        Result AddDeviceDataSubscription(ushort address, ushort numberOfPoints, IDeviceDataMemorySubscription runtimeConfigurationItemViewModel);
        Result AddLocalDataSubscription(ILocalDataMemorySubscription runtimeConfigurationItemViewModel);
        Result TriggerDeviceDataSubscriptionByAddress(ushort triggeredAddress, ushort numberOfPoints);
        Result TriggerLocalDataSubscriptionByViewModel(IEditableValueViewModel triggeredValueViewModel);

    }

    public interface IMemorySubscription
    {
        void Execute();
    }
    public interface IDeviceDataMemorySubscription : IMemorySubscription
    {
        
    }

    public interface ILocalDataMemorySubscription : IMemorySubscription
    {
        IEditableValueViewModel EditableValueViewModel { get; }
    }
}