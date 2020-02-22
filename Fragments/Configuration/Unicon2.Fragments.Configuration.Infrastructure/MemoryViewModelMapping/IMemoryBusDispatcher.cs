using System;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping
{
    public interface IMemoryBusDispatcher : IDisposable
    {
        Result AddSubscription(IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel);
        Result TriggerSubscriptionByAddress(ushort triggeredAddress, IConfigurationItemVisitor<Result> visitor);
    }
}