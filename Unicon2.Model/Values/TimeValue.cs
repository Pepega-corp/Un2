using System;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]

    public class TimeValue : FormattedValueBase, ITimeValue
    {
        public override string StrongName => nameof(TimeValue);

        public override string AsString()
        {
            string resultTimeString = "";
            if (NumberOfPointsInUse > 0)
            {
                resultTimeString += DayInMonthValue.ToString("D2");
            }
            if (NumberOfPointsInUse > 1)
            {
                resultTimeString += ".";
                resultTimeString += MonthValue.ToString("D2"); ;
            }
            if (NumberOfPointsInUse > 2)
            {
                resultTimeString += ".";
                resultTimeString += "20" + YearValue.ToString("D2");
            }
            if (NumberOfPointsInUse > 3)
            {
                resultTimeString += "  ";
                resultTimeString += HoursValue.ToString("D2"); ;
            }
            if (NumberOfPointsInUse > 4)
            {
                resultTimeString += ":";
                resultTimeString += MinutesValue.ToString("D2"); ;
            }
            if (NumberOfPointsInUse > 5)
            {
                resultTimeString += ":";
                resultTimeString += SecondsValue.ToString("D2"); ;
            }
            if (NumberOfPointsInUse > 6)
            {
                resultTimeString += ",";
                resultTimeString += MillisecondsValue.ToString("D" + MillisecondsDecimalsPlaces);
            }


            return resultTimeString;
        }

        [JsonProperty]
        public int MillisecondsDecimalsPlaces { get; set; }
        [JsonProperty]
        public int NumberOfPointsInUse { get; set; }
        [JsonProperty]
        public int YearValue { get; set; }
        [JsonProperty]
        public int MonthValue { get; set; }
        [JsonProperty]
        public int DayInMonthValue { get; set; }
        [JsonProperty]
        public int HoursValue { get; set; }
        [JsonProperty]
        public int MinutesValue { get; set; }
        [JsonProperty]
        public int SecondsValue { get; set; }
        [JsonProperty]
        public int MillisecondsValue { get; set; }
        public string GetPindosFullFormatDateTime()
        {
            if (NumberOfPointsInUse < 7) return String.Empty;
            return
                $"{MonthValue:D2}/{DayInMonthValue:D2}/{YearValue:D2},{HoursValue:D2}:{MinutesValue:D2}:{SecondsValue:D2}.{(MillisecondsValue < 100 ? MillisecondsValue * 10 : MillisecondsValue):D3}";
        }

        public DateTime GetFullDateTime()
        {
            return new DateTime(YearValue<2000? YearValue+2000: YearValue, MonthValue, DayInMonthValue, HoursValue, MinutesValue, SecondsValue,
                MillisecondsValue < 100 ? MillisecondsValue * 10 : MillisecondsValue);
		}
		public override T Accept<T>(IValueVisitor<T> visitor)
		{
			return visitor.VisitTimeValue(this);
		}
	}
}
