using System.Windows.Input;

namespace Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements
{
	public interface IDateTimeMeasuringElementViewModel : IMeasuringElementViewModel
	{
		string Date { get; set; }
		string Time { get; set; }
	    void SetDateTime(string date, string time);
		ICommand SetSystemDateTimeCommand { get; set; }
		ICommand SetTimeCommand { get; set; }
		bool IsInEditMode { get; set; }
	}
}