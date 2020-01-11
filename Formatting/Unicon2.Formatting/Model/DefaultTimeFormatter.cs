using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "DefaultTimeFormatterNS", IsReference = true)]
    public class DefaultTimeFormatter : UshortsFormatterBase, IDefaultTimeFormatter
    {
        private Func<ITimeValue> _timeValueGettingFunc;

        public DefaultTimeFormatter(Func<ITimeValue> timeValueGettingFunc)
        {
            this._timeValueGettingFunc = timeValueGettingFunc;
        }

        public override object Clone()
        {
            DefaultTimeFormatter cloneFormatter = new DefaultTimeFormatter(this._timeValueGettingFunc);
            cloneFormatter.DayInMonthPointNumber = this.DayInMonthPointNumber;
            cloneFormatter.YearPointNumber = this.YearPointNumber;
            cloneFormatter.MonthPointNumber = this.MonthPointNumber;
            cloneFormatter.HoursPointNumber = this.HoursPointNumber;
            cloneFormatter.MinutesPointNumber = this.MinutesPointNumber;
            cloneFormatter.SecondsPointNumber = this.SecondsPointNumber;
            cloneFormatter.MillisecondsDecimalsPlaces = this.MillisecondsDecimalsPlaces;
            cloneFormatter.MillisecondsPointNumber = this.MillisecondsPointNumber;
            cloneFormatter.NumberOfPointsInUse = this.NumberOfPointsInUse;

            return cloneFormatter;
        }

        public override string StrongName => nameof(DefaultTimeFormatter);

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            throw new NotImplementedException();
        }

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            ITimeValue value = this._timeValueGettingFunc();
            value.NumberOfPointsInUse = this.NumberOfPointsInUse;
            value.MillisecondsDecimalsPlaces = this.MillisecondsDecimalsPlaces;
            for (int i = 0; i < this.NumberOfPointsInUse; i++)
            {
                if (this.YearPointNumber == i)
                {
                    value.YearValue = ushorts[i];
                }
                else if (this.MonthPointNumber == i)
                {
                    value.MonthValue = ushorts[i];
                }
                else if (this.DayInMonthPointNumber == i)
                {
                    value.DayInMonthValue = ushorts[i];
                }
                else if (this.HoursPointNumber == i)
                {
                    value.HoursValue = ushorts[i];
                }
                else if (this.MinutesPointNumber == i)
                {
                    value.MinutesValue = ushorts[i];
                }
                else if (this.SecondsPointNumber == i)
                {
                    value.SecondsValue = ushorts[i];
                }
                else if (this.MillisecondsPointNumber == i)
                {
                    value.MillisecondsValue = ushorts[i];
                }

            }
            return value;
        }

        [DataMember]
        public int MillisecondsDecimalsPlaces { get; set; }
        [DataMember]
        public int NumberOfPointsInUse { get; set; }
        [DataMember]
        public int YearPointNumber { get; set; }
        [DataMember]
        public int MonthPointNumber { get; set; }
        [DataMember]
        public int DayInMonthPointNumber { get; set; }
        [DataMember]
        public int HoursPointNumber { get; set; }
        [DataMember]
        public int MinutesPointNumber { get; set; }
        [DataMember]
        public int SecondsPointNumber { get; set; }
        [DataMember]
        public int MillisecondsPointNumber { get; set; }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._timeValueGettingFunc = container.Resolve<Func<ITimeValue>>();
            base.InitializeFromContainer(container);
        }
    }
}
