using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Infrastructure.Model
{
    public interface IDefaultTimeFormatter:IUshortsFormatter,IInitializableFromContainer
    {
        int MillisecondsDecimalsPlaces { get; set; }
        int NumberOfPointsInUse { get; set; }
        int YearPointNumber { get; set; }
        int MonthPointNumber { get; set; }
        int DayInMonthPointNumber { get; set; }
        int HoursPointNumber { get; set; }
        int MinutesPointNumber { get; set; }
        int SecondsPointNumber { get; set; }
        int MillisecondsPointNumber { get; set; }

      

    }
}