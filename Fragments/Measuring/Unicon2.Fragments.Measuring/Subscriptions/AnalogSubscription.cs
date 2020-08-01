using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
    public class AnalogSubscription
    {
        private readonly IAnalogMeasuringElement _analogMeasuringElement;
        private readonly IAnalogMeasuringElementViewModel _analogMeasuringElementViewModel;
        private readonly DeviceContext _deviceContext;
        private readonly IFormattingService _formattingService;

        public AnalogSubscription(IAnalogMeasuringElement analogMeasuringElement,
            IAnalogMeasuringElementViewModel analogMeasuringElementViewModel, DeviceContext deviceContext,
            string groupName, IFormattingService formattingService)
        {
            this._analogMeasuringElement = analogMeasuringElement;
            this._analogMeasuringElementViewModel = analogMeasuringElementViewModel;
            _deviceContext = deviceContext;
            this._formattingService = formattingService;
            GroupName = groupName;
        }




        private async Task ApplyUshortOnAnalog(ushort[] result)
        {
            IFormattedValue value =
               await this._formattingService.FormatValueAsenc(this.AnalogMeasuringElement.UshortsFormatter, result,this._deviceContext);
            ApplyValue(value);
        }


        private void ApplyValue(IFormattedValue value)
        {
            this._analogMeasuringElementViewModel.FormattedValueViewModel = StaticContainer.Container
                .Resolve<IValueViewModelFactory>().CreateFormattedValueViewModel(value);
        }

        public async Task Execute()
        {
            if (_deviceContext.DeviceMemory.DeviceMemoryValues.ContainsKey(this.AnalogMeasuringElement.Address))
            {
               await this.ApplyUshortOnAnalog(new ushort[]
                    {_deviceContext.DeviceMemory.DeviceMemoryValues[this.AnalogMeasuringElement.Address]});
            }
            else
            {
                var res = await _deviceContext.DataProviderContainer.DataProvider.ReadHoldingResgistersAsync(
                    this.AnalogMeasuringElement.Address, this.AnalogMeasuringElement.NumberOfPoints,
                    "Read analog: " + this.AnalogMeasuringElement.Name);
                if (res.IsSuccessful)
                {
                  await  this.ApplyUshortOnAnalog(res.Result);
                }
            }

        }

        public string GroupName { get; }

        public IAnalogMeasuringElement AnalogMeasuringElement
        {
            get { return this._analogMeasuringElement; }
        }
    }
}