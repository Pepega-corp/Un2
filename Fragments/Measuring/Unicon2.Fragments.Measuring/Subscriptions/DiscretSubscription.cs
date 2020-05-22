using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.ViewModel.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public class DiscreteSubscription : IMeasuringMemorySubscription
	{
		private IDiscretMeasuringElementViewModel _discretMeasuringElementViewModel;
		private readonly DeviceContext _deviceContext;
		public IDiscretMeasuringElement DiscretMeasuringElement { get; }

		public DiscreteSubscription(IDiscretMeasuringElement discretMeasuringElement,
			IDiscretMeasuringElementViewModel discretMeasuringElementViewModel, DeviceContext deviceContext,
			string groupName)
		{
			DiscretMeasuringElement = discretMeasuringElement;
			_discretMeasuringElementViewModel = discretMeasuringElementViewModel;
			_deviceContext = deviceContext;
			GroupName = groupName;
		}




		private void ApplyUshortOnDiscret(ushort result)
		{
			bool valueFromUshort =
				result.GetBoolArrayFromUshort()[DiscretMeasuringElement.AddressOfBit.BitAddressInWord];
			ApplyValue(valueFromUshort);
		}


		private void ApplyValue(bool value)
		{
			if (_discretMeasuringElementViewModel.FormattedValueViewModel == null)
			{
				var boolValue = StaticContainer.Container.Resolve<IBoolValue>();
				boolValue.BoolValueProperty = value;
				_discretMeasuringElementViewModel.FormattedValueViewModel = StaticContainer.Container
					.Resolve<IValueViewModelFactory>().CreateFormattedValueViewModel(boolValue);
			}
			else
			{
				((IBoolValueViewModel) _discretMeasuringElementViewModel.FormattedValueViewModel).BoolValueProperty =
					value;
			}
		}

		public async Task Execute()
		{
			if (DiscretMeasuringElement.AddressOfBit.NumberOfFunction == 3)
			{

				if (_deviceContext.DeviceMemory.DeviceMemoryValues.ContainsKey(DiscretMeasuringElement.AddressOfBit
					.Address))
				{
					ApplyUshortOnDiscret(_deviceContext.DeviceMemory.DeviceMemoryValues[DiscretMeasuringElement
						.AddressOfBit
						.Address]);
				}
				else
				{
					var res = await _deviceContext.DataProviderContaining.DataProvider.ReadHoldingResgistersAsync(
						DiscretMeasuringElement.AddressOfBit
							.Address, DiscretMeasuringElement.NumberOfPoints,
						"Read discret: " + DiscretMeasuringElement.Name);
					if (res.IsSuccessful)
					{
						ApplyUshortOnDiscret(res.Result.First());
					}
				}
			}

			if (DiscretMeasuringElement.AddressOfBit.NumberOfFunction == 1)
			{
				var res = await _deviceContext.DataProviderContaining.DataProvider.ReadCoilStatusAsync(
					DiscretMeasuringElement.AddressOfBit.Address, "Read discret: " + DiscretMeasuringElement.Name);
				if (res.IsSuccessful)
				{
					ApplyValue(res.Result);
				}
			}
		}

		public string GroupName { get; }
	}
}