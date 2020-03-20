using System;
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
            IMemorySubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory);

        Result AddLocalAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory);

        Result AddSubscriptionById(IMemorySubscription subscription, Guid id);
    }

    public enum MemoryKind
    {
        UshortMemory,
        BoolMemory
    }

}