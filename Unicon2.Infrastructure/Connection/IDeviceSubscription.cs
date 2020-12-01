namespace Unicon2.Infrastructure.Connection
{
    public interface IDeviceSubscription
    {
        void Execute();
        int Priority { get; set; } 
    }
}