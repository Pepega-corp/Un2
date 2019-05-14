using System;

namespace Unicon2.Infrastructure.Values
{
    public interface ITimeValue:IFormattedValue
    {
        int MillisecondsDecimalsPlaces { get; set; }
        int NumberOfPointsInUse { get; set; }
        int YearValue { get; set; }
        int MonthValue { get; set; }
        int DayInMonthValue { get; set; }
        int HoursValue { get; set; }
        int MinutesValue { get; set; }
        int SecondsValue { get; set; }
        int MillisecondsValue { get; set; }
        string GetPindosFullFormatDateTime();

        DateTime GetFullDateTime();
    }
}