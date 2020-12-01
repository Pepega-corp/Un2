using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
    public class DateTimeSubscription : IMeasuringMemorySubscription
    {
        private IDateTimeMeasuringElementViewModel _dateTimeMeasuringElementViewModel;
        private readonly IDateTimeMeasuringElement _dateTimeMeasuringElement;
        private DeviceContext _deviceContext;


        public DateTimeSubscription(IDateTimeMeasuringElementViewModel dateTimeMeasuringElementViewModel,
            IDateTimeMeasuringElement dateTimeMeasuringElement, string groupName, DeviceContext deviceContext)
        {
            this._dateTimeMeasuringElementViewModel = dateTimeMeasuringElementViewModel;
            this._dateTimeMeasuringElement = dateTimeMeasuringElement;
            this.GroupName = groupName;
            this._deviceContext = deviceContext;
        }

        public string GroupName { get; }

        public async Task Execute()
        {
            if (!this._deviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                return;
            }
            var res = await this._deviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(
                this._dateTimeMeasuringElement.StartAddress, 16, "Read datetime");
            if (res.IsSuccessful)
            {
                this._dateTimeMeasuringElementViewModel.SetDateTime(
                    $"{res.Result[2]:00},{res.Result[1]:00},{res.Result[1]:00}",
                    $"{res.Result[3]:00}:{res.Result[4]:00}:{res.Result[5]:00},{res.Result[6]}");
            }
        }
    }
}
