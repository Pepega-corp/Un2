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
            YearPointNumber = "0";
            MonthPointNumber = "1";
            DayInMonthPointNumber = "2";
            HoursPointNumber = "3";
            MinutesPointNumber = "4";
            SecondsPointNumber = "5";
            MillisecondsPointNumber = "6";
            MillisecondsDecimalsPlaces = "2";
            NumberOfPointsInUse = "7";
        }
   
        public override string StrongName => StringKeys.DEFAULT_TIME_FORMATTER;
       
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public string MillisecondsDecimalsPlaces
        {
            get { return _millisecondsDecimalsPlaces; }
            set
            {
                _millisecondsDecimalsPlaces = value;
                RaisePropertyChanged();
            }
        }

        public string NumberOfPointsInUse
        {
            get { return _numberOfPointsInUse; }
            set
            {
                _numberOfPointsInUse = value;
                RaisePropertyChanged();

            }
        }

        public string YearPointNumber
        {
            get { return _yearPointNumber; }
            set
            {
                _yearPointNumber = value;
                RaisePropertyChanged();

            }
        }

        public string MonthPointNumber
        {
            get { return _monthPointNumber; }
            set
            {
                _monthPointNumber = value;
                RaisePropertyChanged();

            }
        }

        public string DayInMonthPointNumber
        {
            get { return _dayInMonthPointNumber; }
            set
            {
                _dayInMonthPointNumber = value;
                RaisePropertyChanged();

            }
        }

        public string HoursPointNumber
        {
            get { return _hoursPointNumber; }
            set
            {
                _hoursPointNumber = value;
                RaisePropertyChanged();

            }
        }

        public string MinutesPointNumber
        {
            get { return _minutesPointNumber; }
            set
            {
                _minutesPointNumber = value;
                RaisePropertyChanged();

            }
        }

        public string SecondsPointNumber
        {
            get { return _secondsPointNumber; }
            set
            {
                _secondsPointNumber = value;
                RaisePropertyChanged();

            }
        }

        public string MillisecondsPointNumber
        {
            get { return _millisecondsPointNumber; }
            set
            {
                _millisecondsPointNumber = value;
                RaisePropertyChanged();

            }
        }
    }
}
