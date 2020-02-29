using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Subscription
{
    public class DeviceEventsDispatcher : IDeviceEventsDispatcher
    {
        private readonly Dictionary<ushort, MemorySubscriptionCollection<IMemorySubscription>>
            _deviceDataObservers;

        private readonly Dictionary<Guid, MemorySubscriptionCollection<IMemorySubscription>>
            _localDataObservers;

        public DeviceEventsDispatcher()
        {
            _deviceDataObservers =
                new Dictionary<ushort, MemorySubscriptionCollection<IMemorySubscription>>();
            _localDataObservers =
                new Dictionary<Guid, MemorySubscriptionCollection<IMemorySubscription>>();
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
            IMemorySubscription deviceDataMemorySubscription)
        {
            if (!_deviceDataObservers.ContainsKey(address))
            {
                _deviceDataObservers.Add(address,
                    new MemorySubscriptionCollection<IMemorySubscription>(deviceDataMemorySubscription));
            }
            else
            {
                if (_deviceDataObservers[address].Collection
                    .All(subscription => subscription != deviceDataMemorySubscription))
                    _deviceDataObservers[address].Collection.Add(deviceDataMemorySubscription);
            }
        }


        public void Dispose()
        {
            _deviceDataObservers.Clear();
            _localDataObservers.Clear();

        }


        public Result AddAddressSubscription(ushort start, ushort length,
            IMemorySubscription memorySubscription)
        {
            var addresses = GetAddressesRelated(start,
                length);
            if (addresses.All(address =>
                _deviceDataObservers.ContainsKey(address) && _deviceDataObservers[address].Collection.Any(
                    subscription =>
                        subscription == memorySubscription)))
            {
                return Result.Create(true);
            }

            foreach (var address in addresses)
            {
                AddDeviceSubscriptionToCollection(address, memorySubscription);
            }

            return Result.Create(true);
        }

        public Result AddSubscriptionById(IMemorySubscription subscription, Guid id)
        {
            if (_localDataObservers.ContainsKey(id))
            {
                if (_localDataObservers[id].Collection
                    .Any(subscriptionExisting => subscriptionExisting == subscription))
                {
                    return Result.Create(true);
                }
                else
                {
                    _localDataObservers[id].Collection
                        .Add(subscription);
                    return Result.Create(true);

                }
            }

            _localDataObservers.Add(id,
                new MemorySubscriptionCollection<IMemorySubscription>(subscription));
            return Result.Create(true);
        }

        public Result TriggerAddressSubscription(ushort triggeredAddress, ushort numberOfPoints)
        {
            List<IMemorySubscription> deviceDataMemorySubscriptions =
                new List<IMemorySubscription>();
            for (var i = triggeredAddress; i < triggeredAddress + numberOfPoints; i++)
            {
                deviceDataMemorySubscriptions.AddRange(_deviceDataObservers[i].Collection);
            }

            deviceDataMemorySubscriptions.Distinct().ToList().ForEach(subscription => subscription.Execute());
            return Result.Create(true);
        }

        public Result TriggerSubscriptionById(Guid id)
        {
            _localDataObservers[id].Collection.ForEach(subscription => subscription.Execute());
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