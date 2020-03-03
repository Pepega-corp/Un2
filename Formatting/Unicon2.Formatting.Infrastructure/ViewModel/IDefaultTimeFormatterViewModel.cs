using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface IDefaultTimeFormatterViewModel : IUshortsFormatterViewModel
    {
        string MillisecondsDecimalsPlaces { get; set; }
        string NumberOfPointsInUse { get; set; }
        string YearPointNumber { get; set; }
        string MonthPointNumber { get; set; }
        string DayInMonthPointNumber { get; set; }
        string HoursPointNumber { get; set; }
        string MinutesPointNumber { get; set; }
        string SecondsPointNumber { get; set; }
        string MillisecondsPointNumber { get; set; }
    }
}