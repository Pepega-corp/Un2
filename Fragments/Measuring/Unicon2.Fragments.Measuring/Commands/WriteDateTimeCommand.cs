using System;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.Commands;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.Commands
{
    public class WriteDateTimeCommand:ICommandFactory
    {
        private readonly DateTimeMeasuringElementViewModel _dateTimeMeasuringElementViewModel;
        private readonly IDateTimeMeasuringElement _dateTimeMeasuringElement;
        private readonly DeviceContext _deviceContext;
        private readonly bool _isSystemTime;

        public WriteDateTimeCommand(IDateTimeMeasuringElementViewModel dateTimeMeasuringElementViewModel,
            IDateTimeMeasuringElement dateTimeMeasuringElement, DeviceContext deviceContext, bool isSystemTime) : base()
        {
            this._dateTimeMeasuringElementViewModel =
                dateTimeMeasuringElementViewModel as DateTimeMeasuringElementViewModel;
            this._dateTimeMeasuringElement = dateTimeMeasuringElement;
            this._deviceContext = deviceContext;
            this._isSystemTime = isSystemTime;
        }

        private bool CanExecute()
        {
            return !this._dateTimeMeasuringElementViewModel.HasErrors||_isSystemTime;
        }

        private async void Execute()
        {
            if (!_deviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                return;
            }


            var ushortstoWrite = new ushort[16];
            for (int i = 0; i < ushortstoWrite.Length; i++)
            {
                ushortstoWrite[i] = 0;
            }

            if (this._isSystemTime)
            {
                var dateTime = DateTime.Now;
                ushortstoWrite[0] = (ushort) (dateTime.Year - 2000);
                ushortstoWrite[1] = (ushort) (dateTime.Month);
                ushortstoWrite[2] = (ushort) (dateTime.Day);
                ushortstoWrite[3] = (ushort) (dateTime.Hour);
                ushortstoWrite[4] = (ushort) (dateTime.Minute);
                ushortstoWrite[5] = (ushort) (dateTime.Second);
                ushortstoWrite[6] = (ushort) (dateTime.Millisecond);

            }
            else
            {
                var dateParts = this._dateTimeMeasuringElementViewModel.Date.Split(',');
                var timeParts = this._dateTimeMeasuringElementViewModel.Time.Split(',', ':');
                ushortstoWrite[2] = ushort.Parse(dateParts[0]);
                ushortstoWrite[1] = ushort.Parse(dateParts[1]);
                ushortstoWrite[0] = ushort.Parse(dateParts[2]);
                ushortstoWrite[3] = ushort.Parse(timeParts[0]);
                ushortstoWrite[4] = ushort.Parse(timeParts[1]);
                ushortstoWrite[5] = ushort.Parse(timeParts[2]);
                ushortstoWrite[6] = ushort.Parse(timeParts[3]);

            }

            var res =
                await this._deviceContext.DataProviderContainer.DataProvider.Item.WriteMultipleRegistersAsync(
                    this._dateTimeMeasuringElement.StartAddress, ushortstoWrite, "Set date time");
            if (res.IsSuccessful)
            {
                this._dateTimeMeasuringElementViewModel.SetDateTime(
                    $"{ushortstoWrite[2]:00},{ushortstoWrite[1]:00},{ushortstoWrite[1]:00}",
                    $"{ushortstoWrite[3]:00}:{ushortstoWrite[4]:00}:{ushortstoWrite[5]:00},{ushortstoWrite[6]}");
            }
        }


        public ICommand CreateCommand()
        {
            return new RelayCommand(Execute,CanExecute);
        }
    }
}
