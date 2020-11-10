using System;

namespace Unicon2.Infrastructure.Services
{
    public interface IGlobalEventManager
    {
        void SendMessage<T>(T message);
        void Subscribe<T>(Action<T> action);
        void Unsubscribe<T>(Action<T> action);
    }
}