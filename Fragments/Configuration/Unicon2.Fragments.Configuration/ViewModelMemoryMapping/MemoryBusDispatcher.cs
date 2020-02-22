using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class MemoryBusDispatcher : IMemoryBusDispatcher
    {
        private readonly Dictionary<ushort, MemorySubscriptionCollection> _observers;

        public MemoryBusDispatcher()
        {
            _observers = new Dictionary<ushort, MemorySubscriptionCollection>();
        }

        private static ushort[] GetAddressesRelated(ushort start, ushort lenght)
        {
            var res = new ushort[lenght];
            var count = 0;
            for (var i = start; i < start + lenght; i++)
            {
                res[count++] = i;
            }
            return res;
        }

        public Result AddSubscription(IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel)
        {
            if (runtimeConfigurationItemViewModel.Model is IProperty property)
            {
                var addresses = GetAddressesRelated(property.Address, property.NumberOfPoints);
                if (addresses.All(address =>
                    _observers.ContainsKey(address) && _observers[address].Collection.Any(subscription =>
                        subscription.Observer == runtimeConfigurationItemViewModel)))
                {
                    return Result.Create(true);
                }

                foreach (var address in addresses)
                {
                    AddPropertyToSubscriptionCollection(address, runtimeConfigurationItemViewModel);
                }

                return Result.Create(true);
            }

            return Result.Create(false);
        }

        private void AddPropertyToSubscriptionCollection(ushort address, IRuntimeConfigurationItemViewModel property)
        {
            if (!_observers.ContainsKey(address))
            {
                _observers.Add(address, new MemorySubscriptionCollection(property));
            }
            else
            {
                if (_observers[address].Collection.All(subscription => subscription.Observer != property))
                    _observers[address].Collection.Add(new MemorySubscription(property));
            }
        }
        public Result TriggerSubscriptionByAddress(ushort triggeredAddress,IConfigurationItemVisitor<Result> visitor)
        {
            return _observers[triggeredAddress].Accept(visitor);
        }

        public void Dispose()
        {
            _observers.Clear();
        }
    }

    public class MemorySubscriptionCollection
    {
        public MemorySubscriptionCollection()
        {
            Collection = new List<MemorySubscription>();
        }

        public MemorySubscriptionCollection(IRuntimeConfigurationItemViewModel initial)
        {
            Collection = new List<MemorySubscription>() {new MemorySubscription(initial)};
        }

        public List<MemorySubscription> Collection { get; }

        public Result Accept(IConfigurationItemVisitor<Result> configurationItemVisitor)
        {
            var res = Result.Create(true);
            foreach (var subscription in Collection)
                res = Result.CreateMergeAnd(res, subscription.Observer.Accept(configurationItemVisitor));
            return res;
        }
    }

    public class MemorySubscription
    {
        public MemorySubscription(IRuntimeConfigurationItemViewModel observer)
        {
            Observer = observer;
        }

        public IRuntimeConfigurationItemViewModel Observer { get; }
    }
}