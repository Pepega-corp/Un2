using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Subscriptions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Measuring.Factories
{
    public interface IMeasuringGroupViewModelFactory
    {
        IMeasuringGroupViewModel CreateMeasuringGroupViewModel(IMeasuringGroup measuringGroup,
	        MeasuringSubscriptionSet measuringSubscriptionSet, DeviceContext deviceContext);
    }
}