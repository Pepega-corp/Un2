using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public interface IMeasuringMemorySubscription
	{
		string GroupName { get; }
		Task Execute();
	}

}
