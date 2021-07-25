using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Subscription
{
    public class FragmentLevelEventsDispatcher : IDeviceEventsDispatcher
    {
        private readonly Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>
            _deviceMemoryObservers;

        private readonly Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>
            _localMemoryDataObservers;

        private readonly Dictionary<Guid, MemorySubscriptionCollection<IDeviceSubscription>>
            _idObservers;


        public FragmentLevelEventsDispatcher()
        {
            _deviceMemoryObservers =
                new Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>();
            _localMemoryDataObservers =
                new Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>();
            _idObservers =
                new Dictionary<Guid, MemorySubscriptionCollection<IDeviceSubscription>>();
        }


        private static ushort[] GetAddressesRelated(ushort start, ushort length)
        {
            var res = new ushort[length];
            var count = 0;
            for (var i = start; i < start + length; i++)
            {
                res[count++] = i;
            }

            return res;
        }

        private void AddDeviceSubscriptionToCollection(ushort address,
            IDeviceSubscription deviceDataMemorySubscription,
            Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>
                memoryDataObservers)
        {
            if (!memoryDataObservers.ContainsKey(address))
            {
                memoryDataObservers.Add(address,
                    new MemorySubscriptionCollection<IDeviceSubscription>(deviceDataMemorySubscription));
            }
            else
            {
                if (memoryDataObservers[address].Collection
                    .All(subscription => subscription != deviceDataMemorySubscription))
                    memoryDataObservers[address].Collection.Add(deviceDataMemorySubscription);
            }
        }

        private Result AddAddressSubscription(ushort start, ushort length,
            IDeviceSubscription memorySubscription,
            Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>
                memoryObservers)
        {
            var addresses = GetAddressesRelated(start,
                length);
            if (addresses.All(address =>
                memoryObservers.ContainsKey(address) && memoryObservers[address].Collection.Any(
                    subscription =>
                        subscription == memorySubscription)))
            {
                return Result.Create(true);
            }

            foreach (var address in addresses)
            {
                AddDeviceSubscriptionToCollection(address, memorySubscription, memoryObservers);
            }

            return Result.Create(true);
        }

        private Result TriggerAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            Dictionary<ushort, MemorySubscriptionCollection<IDeviceSubscription>>
                memoryDataObservers)
        {
            List<IDeviceSubscription> deviceDataMemorySubscriptions =
                new List<IDeviceSubscription>();
            for (var i = triggeredAddress; i < triggeredAddress + numberOfPoints; i++)
            {
                if (memoryDataObservers.ContainsKey(i))
                {
                    deviceDataMemorySubscriptions.AddRange(memoryDataObservers[i].Collection);
                }
            }

            var res =
                deviceDataMemorySubscriptions.Distinct().OrderBy(subscription => subscription.Priority).ToList();
            deviceDataMemorySubscriptions.Distinct().OrderBy(subscription =>subscription.Priority).ToList().ForEach(subscription =>
            {
                subscription.Execute();
            });
            return Result.Create(true);
        }


        public void Dispose()
        {
            _deviceMemoryObservers.Clear();
            _idObservers.Clear();
        }

        public Result AddDeviceAddressSubscription(ushort start, ushort length,
            IDeviceSubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return AddAddressSubscription(start, length,
                memorySubscription, _deviceMemoryObservers);
        }

        public Result AddLocalAddressSubscription(ushort start, ushort length,
            IDeviceSubscription memorySubscription, MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return AddAddressSubscription(start, length,
                memorySubscription, _localMemoryDataObservers);
        }

        public Result AddSubscriptionById(IDeviceSubscription subscription, Guid id)
        {
            if (_idObservers.ContainsKey(id))
            {
                if (_idObservers[id].Collection
                    .Any(subscriptionExisting => subscriptionExisting == subscription))
                {
                    return Result.Create(true);
                }
                else
                {
                    _idObservers[id].Collection
                        .Add(subscription);
                    return Result.Create(true);
                }
            }

            _idObservers.Add(id,
                new MemorySubscriptionCollection<IDeviceSubscription>(subscription));
            return Result.Create(true);
        }

        public void RemoveSubscriptionById(Guid id)
        {
            _idObservers.Remove(id);
        }

        public Result TriggerLocalAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return TriggerAddressSubscription(triggeredAddress, numberOfPoints, _localMemoryDataObservers);
        }


        public Result TriggerDeviceAddressSubscription(ushort triggeredAddress, ushort numberOfPoints,
            MemoryKind memoryKind = MemoryKind.UshortMemory)
        {
            return TriggerAddressSubscription(triggeredAddress, numberOfPoints, _deviceMemoryObservers);
        }

        public Result TriggerSubscriptionById(Guid id)
        {
            _idObservers[id].Collection.ForEach(subscription => subscription.Execute());
            return Result.Create(true);
        }
    }

    public class MemorySubscriptionCollection<T>
    {
        public MemorySubscriptionCollection()
        {
            Collection = new List<T>();
        }

        public MemorySubscriptionCollection(T initial)
        {
            Collection = new List<T>() {initial};
        }

        public List<T> Collection { get; }
    }
}