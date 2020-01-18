using System;

namespace Unicon2.Presentation.Infrastructure.Events
{
    public interface IGlobalEventsService
    {
        void SendMessage<T>(T message);
        void Subscribe<T>(Action<T> action);
        void Unsubscribe<T>(Action<T> action);
    }
}