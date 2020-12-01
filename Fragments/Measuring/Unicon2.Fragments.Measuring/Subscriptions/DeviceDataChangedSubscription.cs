using Unicon2.Infrastructure.Connection;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public class DeviceDataChangedSubscription : IDeviceSubscription
	{
        public int Priority { get; set; } = 1;

		//private IDeviceMemory _deviceMemory;
		//private 

		//public DeviceDataChangedSubscription()
		//{

		//}
		//public void Execute()
		//{
		//	if ((descretMeasuringElement.AddressOfBit.NumberOfFunction == 3) || (descretMeasuringElement.AddressOfBit.NumberOfFunction == 4))
		//	{
		//		BitArray bitarr = new BitArray(new[] { (int)(queryResult.Result)[0] });
		//		bool bitResult = bitarr[(descretMeasuringElement.AddressOfBit).BitAddressInWord];
		//		if (bitResult == descretMeasuringElement.DeviceValue) return;
		//		descretMeasuringElement.DeviceValue = bitResult;
		//		descretMeasuringElement.ElementChangedAction?.Invoke();
		//	}
		//	if (boolQueryResult.IsSuccessful)
		//	{

		//		if (boolQueryResult.Result == this.DeviceValue) return;
		//		this.DeviceValue = boolQueryResult.Result;
		//		this.ElementChangedAction?.Invoke();
		//	}
		//}
		public void Execute()
		{
			throw new System.NotImplementedException();
		}
	}
}