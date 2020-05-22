using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Subscriptions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Measuring.Factories
{
	public class MeasuringMemorySubscriptionFactory
	{
		public MeasuringMemorySubscriptionFactory()
		{

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
		}


		private DiscreteSubscription CreateDiscreteSubscription(
			IDiscretMeasuringElementViewModel discretMeasuringElement, IMeasuringElement measuringElement,
			string groupName, DeviceContext deviceContext)
		{
			return new DiscreteSubscription(measuringElement as IDiscretMeasuringElement, discretMeasuringElement,
				deviceContext, groupName);
		}
	}
}
