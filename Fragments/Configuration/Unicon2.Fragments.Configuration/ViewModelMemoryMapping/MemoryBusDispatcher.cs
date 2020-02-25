using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class MemoryBusDispatcher : IMemoryBusDispatcher
    {
        private readonly Dictionary<ushort, MemorySubscriptionCollection<IDeviceDataMemorySubscription>>
            _deviceDataObservers;

        private readonly Dictionary<Guid, MemorySubscriptionCollection<ILocalDataMemorySubscription>>
            _localDataObservers;

        public MemoryBusDispatcher()
        {
            _deviceDataObservers =
                new Dictionary<ushort, MemorySubscriptionCollection<IDeviceDataMemorySubscription>>();
            _localDataObservers =
                new Dictionary<Guid, MemorySubscriptionCollection<ILocalDataMemorySubscription>>();
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
            IDeviceDataMemorySubscription deviceDataMemorySubscription)
        {
            if (!_deviceDataObservers.ContainsKey(address))
            {
                _deviceDataObservers.Add(address,
                    new MemorySubscriptionCollection<IDeviceDataMemorySubscription>(deviceDataMemorySubscription));
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

        public Result AddDeviceDataSubscription(ushort start, ushort length,
            IDeviceDataMemorySubscription deviceDataMemorySubscription)
        {

            var addresses = GetAddressesRelated(start,
                length);
            if (addresses.All(address =>
                _deviceDataObservers.ContainsKey(address) && _deviceDataObservers[address].Collection.Any(
                    subscription =>
                        subscription == deviceDataMemorySubscription)))
            {
                return Result.Create(true);
            }

            foreach (var address in addresses)
            {
                AddDeviceSubscriptionToCollection(address, deviceDataMemorySubscription);
            }

            return Result.Create(true);
        }

        public Result AddLocalDataSubscription(ILocalDataMemorySubscription localDataMemorySubscription)
        {
            if (_localDataObservers.ContainsKey(localDataMemorySubscription.EditableValueViewModel.Id))
            {
                if (_localDataObservers[localDataMemorySubscription.EditableValueViewModel.Id].Collection
                    .Any(subscription => subscription == localDataMemorySubscription))
                {
                    return Result.Create(true);
                }
                else
                {
                    _localDataObservers[localDataMemorySubscription.EditableValueViewModel.Id].Collection
                        .Add(localDataMemorySubscription);
                    return Result.Create(true);

                }
            }

            _localDataObservers.Add(localDataMemorySubscription.EditableValueViewModel.Id,
                new MemorySubscriptionCollection<ILocalDataMemorySubscription>(localDataMemorySubscription));
            return Result.Create(true);
        }

        public Result TriggerDeviceDataSubscriptionByAddress(ushort triggeredAddress, ushort numberOfPoints)
        {
            List<IDeviceDataMemorySubscription> deviceDataMemorySubscriptions =
                new List<IDeviceDataMemorySubscription>();
            for (var i = triggeredAddress; i < triggeredAddress + numberOfPoints; i++)
            {
                deviceDataMemorySubscriptions.AddRange(_deviceDataObservers[i].Collection);
            }

            deviceDataMemorySubscriptions.Distinct().ToList().ForEach(subscription => subscription.Execute());
            return Result.Create(true);
        }

        public Result TriggerLocalDataSubscriptionById(Guid id)
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