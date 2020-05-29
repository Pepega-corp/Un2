using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
	public class DateTimeMeasuringElementViewModel:MeasuringElementViewModelBase, IDateTimeMeasuringElementViewModel
	{

		public DateTimeMeasuringElementViewModel()
		{
			

		}

		public override string StrongName => MeasuringKeys.DATE_TIME_ELEMENT +
		                                     ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
		public string Date { get; set; }
		public string Time { get; set; }
		public ICommand SetSystemDateTimeCommand { get; }
		public ICommand SetTimeCommand { get; }
		public bool IsInEditMode { get; set; }
	}
}
