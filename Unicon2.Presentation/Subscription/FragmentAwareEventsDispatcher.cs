using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Subscription
{
    public class FragmentAwareEventsDispatcher : IDeviceEventsDispatcher
    {
        private readonly Func<IFragmentViewModel> _getActiveFragment;

        public FragmentAwareEventsDispatcher(Func<IFragmentViewModel> getActiveFragment,
            Dictionary<IFragmentViewModel, IDeviceEventsDispatcher> deviceEventsDispatchersOfFragments)
        {
            _getActiveFragment = getActiveFragment;
            _deviceEventsDispatchersOfFragments = deviceEventsDispatchersOfFragments;
        }

        private Dictionary<IFragmentViewModel, IDeviceEventsDispatcher> _deviceEventsDispatchersOfFragments ;

        public void Dispose()
        {
            _deviceEventsDispatchersOfFragments.ForEach(pair => pair.Value.Dispose());
            _deviceEventsDispatchersOfFragments.Clear();
        }

        public Result AddDeviceAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription runtimeConfigurationItemViewModel)
        {
            throw new NotImplementedException();
        }

        public Result AddLocalAddressSubscription(ushort address, ushort numberOfPoints,
            IMemorySubscription runtimeConfigurationItemViewModel)
        {
            throw new NotImplementedException();
        }

        public Result AddSubscriptionById(IMemorySubscription subscription, Guid id)
        {
            throw new NotImplementedException();
        }

        public Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            throw new NotImplementedException();
        }

        public Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            throw new NotImplementedException();
        }

        public Result TriggerSubscriptionById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}