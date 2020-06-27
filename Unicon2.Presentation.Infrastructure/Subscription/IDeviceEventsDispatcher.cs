using System;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Presentation.Infrastructure.Subscription
{
    public interface IDeviceEventsDispatcher : IDeviceEventsPublisher, IDeviceEventsSubscriber
    {

    }

    public interface IDeviceEventsPublisher
    {
        Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            MemoryKind memoryKind = MemoryKind.UshortMemory);

        Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            MemoryKind memoryKind = MemoryKind.UshortMemory);

        Result TriggerSubscriptionById(Guid id);
    }

    public interface IDeviceEventsSubscriber : IDisposable
    {
        Result AddDeviceAddressSubscription(ushort address, ushort numberOfPoints,
            IDeviceSubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory);

        Result AddLocalAddressSubscription(ushort address, ushort numberOfPoints,
            IDeviceSubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory);

        Result AddSubscriptionById(IDeviceSubscription subscription, Guid id);
        void RemoveSubscriptionById(Guid id);

    }

    public enum MemoryKind
    {
        UshortMemory,
        BoolMemory
    }

}