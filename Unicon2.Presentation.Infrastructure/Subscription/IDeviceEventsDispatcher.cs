using System;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Presentation.Infrastructure.Subscription
{
    public interface IDeviceEventsDispatcher : IDisposable
    {
        Result AddAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription runtimeConfigurationItemViewModel);

        Result AddSubscriptionById(IMemorySubscription subscription, Guid id);
        Result TriggerAddressSubscription(ushort triggeredAddress, ushort numberOfPoints);
        Result TriggerSubscriptionById(Guid id);
    }
}
