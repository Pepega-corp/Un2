using System;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;

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


        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitTimeFormatter(this);
        }
        public DefaultTimeFormatterViewModel()
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
        }
   
        public override string StrongName => StringKeys.DEFAULT_TIME_FORMATTER;
       
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
