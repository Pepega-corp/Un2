using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class MemoryBusDispatcher<T> : IDisposable
    {
        private readonly IConfigurationItemVisitor<T> _configurationItemVisitor;
        public Dictionary<ushort, MemorySubscriptionCollection> Observers { get; }

        public MemoryBusDispatcher(IConfigurationItemVisitor<T> configurationItemVisitor)
        {
            _configurationItemVisitor = configurationItemVisitor;
            Observers = new Dictionary<ushort, MemorySubscriptionCollection>();
        }

        public Result AddSubscription(IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel)
        {
            if (runtimeConfigurationItemViewModel.Model is IProperty property)
            {
                var addresses = new ushort[property.NumberOfPoints];
                if (addresses.All(address =>
                    Observers.ContainsKey(address) && Observers[address].Collection.Any(subscription =>
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
            if (!Observers.ContainsKey(address))
            {
                Observers.Add(address, new MemorySubscriptionCollection(property));
            }
            else
            {
                Observers[address].Collection.Add(new MemorySubscription(property));
            }

        }


        //public Result RemoveSubscription(IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel)
        //{
        //       
        //}

        public T TriggerSubscriptionByAddress(ushort triggeredAddress)
        {
            return Observers[triggeredAddress].Accept(_configurationItemVisitor);
        }

        public void Dispose()
        {

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

        public T Accept<T>(IConfigurationItemVisitor<T> configurationItemVisitor)
        {
            throw new NotImplementedException();
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