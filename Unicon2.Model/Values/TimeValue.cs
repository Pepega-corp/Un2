using System;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class TimeValue : FormattedValueBase, ITimeValue
    {
        public override string StrongName => nameof(TimeValue);

        public override string AsString()
        {
            string resultTimeString = "";
            if (this.NumberOfPointsInUse > 0)
            {
                resultTimeString += this.DayInMonthValue.ToString("D2");
            }
            if (this.NumberOfPointsInUse > 1)
            {
                resultTimeString += ".";
                resultTimeString += this.MonthValue.ToString("D2"); ;
            }
            if (this.NumberOfPointsInUse > 2)
            {
                resultTimeString += ".";
                resultTimeString += "20" + this.YearValue.ToString("D2");
            }
            if (this.NumberOfPointsInUse > 3)
            {
                resultTimeString += "  ";
                resultTimeString += this.HoursValue.ToString("D2"); ;
            }
            if (this.NumberOfPointsInUse > 4)
            {
                resultTimeString += ":";
                resultTimeString += this.MinutesValue.ToString("D2"); ;
            }
            if (this.NumberOfPointsInUse > 5)
            {
                resultTimeString += ":";
                resultTimeString += this.SecondsValue.ToString("D2"); ;
            }
            if (this.NumberOfPointsInUse > 6)
            {
                resultTimeString += ",";
                resultTimeString += this.MillisecondsValue.ToString("D" + this.MillisecondsDecimalsPlaces);
            }


            return resultTimeString;
        }

        [DataMember]
        public int MillisecondsDecimalsPlaces { get; set; }
        [DataMember]
        public int NumberOfPointsInUse { get; set; }
        [DataMember]
        public int YearValue { get; set; }
        [DataMember]
        public int MonthValue { get; set; }
        [DataMember]
        public int DayInMonthValue { get; set; }
        [DataMember]
        public int HoursValue { get; set; }
        [DataMember]
        public int MinutesValue { get; set; }
        [DataMember]
        public int SecondsValue { get; set; }
        [DataMember]
        public int MillisecondsValue { get; set; }
        public string GetPindosFullFormatDateTime()
        {
            if (this.NumberOfPointsInUse < 7) return String.Empty;
            return
                $"{this.MonthValue:D2}/{this.DayInMonthValue:D2}/{this.YearValue:D2},{this.HoursValue:D2}:{this.MinutesValue:D2}:{this.SecondsValue:D2}.{(this.MillisecondsValue < 100 ? this.MillisecondsValue * 10 : this.MillisecondsValue):D3}";
        }

        public DateTime GetFullDateTime()
        {
            return new DateTime(this.YearValue<2000? this.YearValue+2000: this.YearValue, this.MonthValue, this.DayInMonthValue, this.HoursValue, this.MinutesValue, this.SecondsValue,
                this.MillisecondsValue < 100 ? this.MillisecondsValue * 10 : this.MillisecondsValue);
		}
		public override T Accept<T>(IValueVisitor<T> visitor)
		{
			return visitor.VisitTimeValue(this);
		}
	}
}
