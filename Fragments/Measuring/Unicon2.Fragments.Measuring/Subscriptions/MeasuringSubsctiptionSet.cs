using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public class MeasuringSubscriptionSet
	{
		public MeasuringSubscriptionSet()
		{
			DiscreteSubscriptions =new List<DiscreteSubscription>();
		}

		public List<DiscreteSubscription> DiscreteSubscriptions { get; }

	}
}
