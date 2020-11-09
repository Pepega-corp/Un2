namespace Unicon2.Presentation.Infrastructure.Subscription
{
    public interface IWeakEventsDispatcher
    {
        void Subscribe<T>(T newSubscriber);
        void TriggerSubscriptions();
    }
    
}