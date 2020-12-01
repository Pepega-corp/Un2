using System;
using System.Globalization;
using System.Windows.Data;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;

namespace Unicon2.Fragments.Measuring.Editor.Converters
{
	public class PresentationElementNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is PresentationElementViewModel presentationElement)
			{
				if (presentationElement.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel
					    presentationGroupViewModel && !string.IsNullOrEmpty(presentationGroupViewModel.Header))
				{
					return presentationGroupViewModel.Header;
				}
				if (presentationElement.TemplatedViewModelToShowOnCanvas is IMeasuringElementViewModel
						measuringElementViewModel && !string.IsNullOrEmpty(measuringElementViewModel.Header))
				{
					return measuringElementViewModel.Header;
				}
			}

			return "Unnamed";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}