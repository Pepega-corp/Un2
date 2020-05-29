using System.Windows.Input;

namespace Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements
{
	public interface IDateTimeMeasuringElementViewModel : IMeasuringElementViewModel
	{
		string Date { get; set; }
		string Time { get; set; }
		ICommand SetSystemDateTimeCommand { get; }
		ICommand SetTimeCommand { get; }
		bool IsInEditMode { get; set; }
	}
}