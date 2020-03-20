using System;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Subscription
{
    public class FragmentEventsDispatcher : IDeviceEventsDispatcher
    {
        private readonly DeviceLevelEventsPublisher _deviceLevelEventsPublisher;
        private readonly FragmentLevelEventsDispatcher _fragmentLevelEventsDispatcher;

        public FragmentEventsDispatcher(DeviceLevelEventsPublisher deviceLevelEventsPublisher,
            FragmentLevelEventsDispatcher fragmentLevelEventsDispatcher)
        {
            _deviceLevelEventsPublisher = deviceLevelEventsPublisher;
            _fragmentLevelEventsDispatcher = fragmentLevelEventsDispatcher;
        }

        public Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return _deviceLevelEventsPublisher.TriggerDeviceAddressSubscription(triggeredAddress, numberOfPoints, memoryKind);
        }

        public Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return _deviceLevelEventsPublisher.TriggerLocalAddressSubscription(triggeredAddress, numberOfPoints, memoryKind);
        }

        public Result TriggerSubscriptionById(Guid id)
        {
            return _deviceLevelEventsPublisher.TriggerSubscriptionById(id);
        }

        public void Dispose()
        {
            _deviceLevelEventsPublisher.Dispose();
            _fragmentLevelEventsDispatcher.Dispose();
        }

        public Result AddDeviceAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return _fragmentLevelEventsDispatcher.AddDeviceAddressSubscription(address, numberOfPoints,
                memorySubscription, memoryKind);
        }

        public Result AddLocalAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return _fragmentLevelEventsDispatcher.AddLocalAddressSubscription(address, numberOfPoints,
                memorySubscription, memoryKind);
        }

        public Result AddSubscriptionById(IMemorySubscription subscription, Guid id)
        {
            return _fragmentLevelEventsDispatcher.AddSubscriptionById(subscription, id);
        }
    }
}