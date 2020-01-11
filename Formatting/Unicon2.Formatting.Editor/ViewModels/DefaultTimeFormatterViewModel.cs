using System;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DefaultTimeFormatterViewModel : UshortsFormatterViewModelBase, IDefaultTimeFormatterViewModel
    {
        private IDefaultTimeFormatter _defaultTimeFormatter;
        private string _millisecondsDecimalsPlaces;
        private string _numberOfPointsInUse;
        private string _yearPointNumber;
        private string _monthPointNumber;
        private string _dayInMonthPointNumber;
        private string _hoursPointNumber;
        private string _minutesPointNumber;
        private string _secondsPointNumber;
        private string _millisecondsPointNumber;



        public DefaultTimeFormatterViewModel(ITypesContainer container)
        {
            this.YearPointNumber = "0";
            this.MonthPointNumber = "1";
            this.DayInMonthPointNumber = "2";
            this.HoursPointNumber = "3";
            this.MinutesPointNumber = "4";
            this.SecondsPointNumber = "5";
            this.MillisecondsPointNumber = "6";
            this.MillisecondsDecimalsPlaces = "2";
            this.NumberOfPointsInUse = "7";
            this._defaultTimeFormatter = container.Resolve<IUshortsFormatter>(StringKeys.DEFAULT_TIME_FORMATTER) as IDefaultTimeFormatter;
        }


        public override IUshortsFormatter GetFormatter()
        {
            this._defaultTimeFormatter.NumberOfPointsInUse = int.Parse(this.NumberOfPointsInUse);
            this._defaultTimeFormatter.MillisecondsDecimalsPlaces = int.Parse(this.MillisecondsDecimalsPlaces);
            this._defaultTimeFormatter.YearPointNumber = int.Parse(this.YearPointNumber);
            this._defaultTimeFormatter.MonthPointNumber = int.Parse(this.MonthPointNumber);
            this._defaultTimeFormatter.DayInMonthPointNumber = int.Parse(this.DayInMonthPointNumber);
            this._defaultTimeFormatter.HoursPointNumber = int.Parse(this.HoursPointNumber);
            this._defaultTimeFormatter.MinutesPointNumber = int.Parse(this.MinutesPointNumber);
            this._defaultTimeFormatter.SecondsPointNumber = int.Parse(this.SecondsPointNumber);
            this._defaultTimeFormatter.MillisecondsPointNumber = int.Parse(this.MillisecondsPointNumber);
            return this._defaultTimeFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            if (ushortsFormatter is IDefaultTimeFormatter)
            {
                IDefaultTimeFormatter settingDefaultTimeFormatter = ushortsFormatter as IDefaultTimeFormatter;
                this.MillisecondsDecimalsPlaces = settingDefaultTimeFormatter.MillisecondsDecimalsPlaces.ToString();
                this.NumberOfPointsInUse = settingDefaultTimeFormatter.NumberOfPointsInUse.ToString();
                this.YearPointNumber = settingDefaultTimeFormatter.YearPointNumber.ToString();
                this.MonthPointNumber = settingDefaultTimeFormatter.MonthPointNumber.ToString();
                this.DayInMonthPointNumber = settingDefaultTimeFormatter.DayInMonthPointNumber.ToString();
                this.HoursPointNumber = settingDefaultTimeFormatter.HoursPointNumber.ToString();
                this.MinutesPointNumber = settingDefaultTimeFormatter.MinutesPointNumber.ToString();
                this.SecondsPointNumber = settingDefaultTimeFormatter.SecondsPointNumber.ToString();
                this.MillisecondsPointNumber = settingDefaultTimeFormatter.MillisecondsPointNumber.ToString();
                this.Model = ushortsFormatter;
            }
        }

        public override string StrongName => StringKeys.DEFAULT_TIME_FORMATTER;
        public override object Model
        {
            get
            {
                return this._defaultTimeFormatter;

            }
            set
            {
                IDefaultTimeFormatter defaultTimeFormatter = value as IDefaultTimeFormatter;

                this._defaultTimeFormatter = defaultTimeFormatter;
            }
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public string MillisecondsDecimalsPlaces
        {
            get { return this._millisecondsDecimalsPlaces; }
            set
            {
                this._millisecondsDecimalsPlaces = value;
                this.RaisePropertyChanged();
            }
        }

        public string NumberOfPointsInUse
        {
            get { return this._numberOfPointsInUse; }
            set
            {
                this._numberOfPointsInUse = value;
                this.RaisePropertyChanged();

            }
        }

        public string YearPointNumber
        {
            get { return this._yearPointNumber; }
            set
            {
                this._yearPointNumber = value;
                this.RaisePropertyChanged();

            }
        }

        public string MonthPointNumber
        {
            get { return this._monthPointNumber; }
            set
            {
                this._monthPointNumber = value;
                this.RaisePropertyChanged();

            }
        }

        public string DayInMonthPointNumber
        {
            get { return this._dayInMonthPointNumber; }
            set
            {
                this._dayInMonthPointNumber = value;
                this.RaisePropertyChanged();

            }
        }

        public string HoursPointNumber
        {
            get { return this._hoursPointNumber; }
            set
            {
                this._hoursPointNumber = value;
                this.RaisePropertyChanged();

            }
        }

        public string MinutesPointNumber
        {
            get { return this._minutesPointNumber; }
            set
            {
                this._minutesPointNumber = value;
                this.RaisePropertyChanged();

            }
        }

        public string SecondsPointNumber
        {
            get { return this._secondsPointNumber; }
            set
            {
                this._secondsPointNumber = value;
                this.RaisePropertyChanged();

            }
        }

        public string MillisecondsPointNumber
        {
            get { return this._millisecondsPointNumber; }
            set
            {
                this._millisecondsPointNumber = value;
                this.RaisePropertyChanged();

            }
        }
    }
}
