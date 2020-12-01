using System.Collections.Generic;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public class MeasuringSubscriptionSet
	{
		public MeasuringSubscriptionSet()
		{
			DiscreteSubscriptions =new List<DiscreteSubscription>();
		    this.AnalogSubscriptions=new List<AnalogSubscription>();
		    this.DateTimeSubscriptions=new List<DateTimeSubscription>();
		}
		public List<AnalogSubscription> AnalogSubscriptions { get; }

		public List<DiscreteSubscription> DiscreteSubscriptions { get; }
		public List<DateTimeSubscription> DateTimeSubscriptions { get; }

	}
}
