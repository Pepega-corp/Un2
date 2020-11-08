using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultTimeFormatter : UshortsFormatterBase, IDefaultTimeFormatter
    {

        public DefaultTimeFormatter()
        {
        }

        public override object Clone()
        {
            DefaultTimeFormatter cloneFormatter = new DefaultTimeFormatter();
            cloneFormatter.DayInMonthPointNumber = DayInMonthPointNumber;
            cloneFormatter.YearPointNumber = YearPointNumber;
            cloneFormatter.MonthPointNumber = MonthPointNumber;
            cloneFormatter.HoursPointNumber = HoursPointNumber;
            cloneFormatter.MinutesPointNumber = MinutesPointNumber;
            cloneFormatter.SecondsPointNumber = SecondsPointNumber;
            cloneFormatter.MillisecondsDecimalsPlaces = MillisecondsDecimalsPlaces;
            cloneFormatter.MillisecondsPointNumber = MillisecondsPointNumber;
            cloneFormatter.NumberOfPointsInUse = NumberOfPointsInUse;

            return cloneFormatter;
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitTimeFormatter(this);
        }

        [JsonProperty]
        public int MillisecondsDecimalsPlaces { get; set; }
        [JsonProperty]
        public int NumberOfPointsInUse { get; set; }
        [JsonProperty]
        public int YearPointNumber { get; set; }
        [JsonProperty]
        public int MonthPointNumber { get; set; }
        [JsonProperty]
        public int DayInMonthPointNumber { get; set; }
        [JsonProperty]
        public int HoursPointNumber { get; set; }
        [JsonProperty]
        public int MinutesPointNumber { get; set; }
        [JsonProperty]
        public int SecondsPointNumber { get; set; }
        [JsonProperty]
        public int MillisecondsPointNumber { get; set; }
    }
}
