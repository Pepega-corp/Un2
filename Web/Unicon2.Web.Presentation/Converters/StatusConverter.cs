using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Unicon2.Web.Presentation.Converters
{
	public class StatusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool) value)
			{
				return Brushes.Green;
			}
			return Brushes.DarkRed;

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
