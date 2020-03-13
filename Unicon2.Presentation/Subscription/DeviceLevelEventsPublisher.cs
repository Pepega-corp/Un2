using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Subscription
{
    public class DeviceLevelEventsPublisher : IDeviceEventsPublisher
    {
        private readonly Func<IFragmentViewModel> _getActiveFragment;
        private Dictionary<IFragmentViewModel, IDeviceEventsDispatcher> _fragmentAwareDispatchers;

        public DeviceLevelEventsPublisher(Func<IFragmentViewModel> getActiveFragment)
        {
            _getActiveFragment = getActiveFragment;
        }

        public void AddFragmentDispatcher(IFragmentViewModel fragmentViewModel,
            IDeviceEventsDispatcher deviceEventsDispatcher)
        {
            _fragmentAwareDispatchers.Add(fragmentViewModel, deviceEventsDispatcher);
        }

        public void Dispose()
        {
            _fragmentAwareDispatchers.ForEach(pair => pair.Value.Dispose());
        }

        public Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            return _fragmentAwareDispatchers[_getActiveFragment()]
                .TriggerLocalAddressSubscription(triggeredAddress, numberOfPoints);
        }



        public Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            return _fragmentAwareDispatchers[_getActiveFragment()]
                .TriggerDeviceAddressSubscription(triggeredAddress, numberOfPoints);
        }

        public Result TriggerSubscriptionById(Guid id)
        {
            return _fragmentAwareDispatchers[_getActiveFragment()]
                .TriggerSubscriptionById(id);
        }
    }
}