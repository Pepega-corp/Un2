namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IFlyoutService
    {
        void CloseFlyout();
        void RegisterFlyout(IFlyoutProvider flyoutProvider);
        void UnregisterFlyout(IFlyoutProvider flyoutProvider);
    }

    public interface IFlyoutProvider
    {
        bool IsFlyoutOpen { get; set; }
    }
}