using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Unicon2.Presentation.Infrastructure.Events;

namespace Unicon2.Shell.EventAggregator
{
    public class GlobalEventsService : IGlobalEventsService
    {
        private readonly IEventAggregator _eventAggregator;

        public GlobalEventsService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private PubSubEvent<T> GetEventBus<T>()
        {
            var bus = _eventAggregator.GetEvent<PubSubEvent<T>>();
            return bus;
        }

        public void SendMessage<T>(T message)
        {
            var bus = GetEventBus<T>();
            bus.Publish(message);
        }

        public void Subscribe<T>(Action<T> action)
        {
            var bus = GetEventBus<T>();
            bus.Subscribe(action,false);
        }

        public void Subscribe<T>(Action<T> action, bool keepSubscriberReferenceAlive)
        {
            var bus = GetEventBus<T>();
            bus.Subscribe(action, keepSubscriberReferenceAlive);
        }

        public void Subscribe<T>(Action<T> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<T> where)
        {
            var bus = GetEventBus<T>();
            bus.Subscribe(action, threadOption, keepSubscriberReferenceAlive, where);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            var bus = GetEventBus<T>();
            bus.Unsubscribe(action);
        }

        // You'd probably have more overloads but I'll wait for C# 4's Optional keyword :) 
    }
}
