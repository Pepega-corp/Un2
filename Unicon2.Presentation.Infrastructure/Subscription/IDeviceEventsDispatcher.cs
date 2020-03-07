using System;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Presentation.Infrastructure.Subscription
{
    public interface IDeviceEventsDispatcher : IDisposable
    {
        Result AddDeviceAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription runtimeConfigurationItemViewModel);
        Result AddLocalAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription runtimeConfigurationItemViewModel);
        Result AddSubscriptionById(IMemorySubscription subscription, Guid id);
        Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints);
        Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints);
        Result TriggerSubscriptionById(Guid id);
    }
}
