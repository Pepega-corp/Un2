using System;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Helpers;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
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
		    var action = new Action<bool>(this.ApplyValue);
			(await DiscretMeasuringElement.GetDiscretElementValue(_deviceContext)).OnSuccess(action);
		}

		public string GroupName { get; }
	}
}