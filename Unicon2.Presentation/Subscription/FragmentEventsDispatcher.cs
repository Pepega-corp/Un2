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

        public Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            return _deviceLevelEventsPublisher.TriggerDeviceAddressSubscription(triggeredAddress, numberOfPoints);
        }

        public Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            return _deviceLevelEventsPublisher.TriggerLocalAddressSubscription(triggeredAddress, numberOfPoints);
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
            IMemorySubscription memorySubscription)
        {
            return _fragmentLevelEventsDispatcher.AddDeviceAddressSubscription(address, numberOfPoints,
                memorySubscription);
        }

        public Result AddLocalAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription memorySubscription)
        {
            return _fragmentLevelEventsDispatcher.AddLocalAddressSubscription(address, numberOfPoints,
                memorySubscription);
        }

        public Result AddSubscriptionById(IMemorySubscription subscription, Guid id)
        {
            return _fragmentLevelEventsDispatcher.AddSubscriptionById(subscription, id);
        }
    }
}