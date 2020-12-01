using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Subscriptions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Measuring.Factories
{
    public class MeasuringMemorySubscriptionFactory
    {
        private readonly IFormattingService _formattingService;

        public MeasuringMemorySubscriptionFactory(IFormattingService formattingService)
        {
            this._formattingService = formattingService;
        }


        public void AddSubscription(MeasuringSubscriptionSet measuringSubscriptionSet,
            IMeasuringElementViewModel measuringElementViewModel, IMeasuringElement measuringElement, string groupName,
            DeviceContext deviceContext)
        {
            if (measuringElementViewModel is IDiscretMeasuringElementViewModel discretMeasuringElement)
            {
                measuringSubscriptionSet.DiscreteSubscriptions.Add(CreateDiscreteSubscription(discretMeasuringElement,
                    measuringElement, groupName, deviceContext));
            }
            else if (measuringElementViewModel is IAnalogMeasuringElementViewModel analogMeasuringElement)
            {
                measuringSubscriptionSet.AnalogSubscriptions.Add(CreateAnalogSubscription(analogMeasuringElement,
                    measuringElement, groupName, deviceContext));
            }
            else if (measuringElementViewModel is IDateTimeMeasuringElementViewModel dateTimeMeasuringElementViewModel)
            {
                measuringSubscriptionSet.DateTimeSubscriptions.Add(
                    CreateDateTimeSubscription(dateTimeMeasuringElementViewModel, measuringElement, groupName, deviceContext));
            }
        }

        private AnalogSubscription CreateAnalogSubscription(
            IAnalogMeasuringElementViewModel analogMeasuringElementViewModel, IMeasuringElement measuringElement,
            string groupName, DeviceContext deviceContext)
        {
            return new AnalogSubscription(measuringElement as IAnalogMeasuringElement, analogMeasuringElementViewModel,
                deviceContext, groupName, this._formattingService);
        }

        private DiscreteSubscription CreateDiscreteSubscription(
            IDiscretMeasuringElementViewModel discretMeasuringElement, IMeasuringElement measuringElement,
            string groupName, DeviceContext deviceContext)
        {
            return new DiscreteSubscription(measuringElement as IDiscretMeasuringElement, discretMeasuringElement,
                deviceContext, groupName);
        }

        private DateTimeSubscription CreateDateTimeSubscription(
            IDateTimeMeasuringElementViewModel dateTimeMeasuringElementViewModel,IMeasuringElement dateTimeMeasuringElement, string groupName,
            DeviceContext deviceContext)
        {
            return new DateTimeSubscription(dateTimeMeasuringElementViewModel,dateTimeMeasuringElement as IDateTimeMeasuringElement, groupName,
                deviceContext);
        }
    }
}