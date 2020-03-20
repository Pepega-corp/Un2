using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Model.Elements;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Fragments.Measuring.MemoryAccess
{
	public class MeasuringLoader
	{
		private IDataProviderContaining _dataProviderContaining;
		private readonly IDeviceEventsDispatcher _deviceEventsDispatcher;

		public MeasuringLoader(IDataProviderContaining dataProviderContaining, IDeviceEventsDispatcher deviceEventsDispatcher)
		{
			_dataProviderContaining = dataProviderContaining;
			_deviceEventsDispatcher = deviceEventsDispatcher;
		}

		public void StartLoading()
		{

		}

		public void StopLoading()
		{

		}


		private Task ExecuteLoadAllElements()
		{

		}
		private async Task ExecuteLoadDescretMeasuringElement(DescretMeasuringElement descretMeasuringElement)
		{
			if ((descretMeasuringElement.AddressOfBit.NumberOfFunction == 3) || (descretMeasuringElement.AddressOfBit.NumberOfFunction == 4))
			{
				IQueryResult<ushort[]> queryResult =
					await _dataProviderContaining.DataProvider.ReadHoldingResgistersAsync(descretMeasuringElement.AddressOfBit.Address, 1, "Read");
				if (queryResult.IsSuccessful)
				{
					_deviceEventsDispatcher.
				}
			}
			else if ((this.AddressOfBit.NumberOfFunction == 1) || (this.AddressOfBit.NumberOfFunction == 2))
			{
				IQueryResult<bool> boolQueryResult =
					await this._dataProvider.ReadCoilStatusAsync(this.AddressOfBit.Address, MeasuringKeys.READ_SINGLE_BIT_QUERY);
				if (boolQueryResult.IsSuccessful)
				{

					if (boolQueryResult.Result == this.DeviceValue) return;
					this.DeviceValue = boolQueryResult.Result;
					this.ElementChangedAction?.Invoke();
				}
			}
		}
		private Task ExecuteLoadAllElements()
		{

		}
		private Task ExecuteLoadAllElements()
		{

		}
	}
}
