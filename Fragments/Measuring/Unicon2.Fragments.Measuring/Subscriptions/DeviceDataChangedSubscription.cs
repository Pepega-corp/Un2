using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public class DeviceDataChangedSubscription: IMemorySubscription
	{
		private IDeviceMemory _deviceMemory;
		private 

		public DeviceDataChangedSubscription()
		{
			
		}
		public void Execute()
		{
			throw new NotImplementedException();
		}
	}
}
