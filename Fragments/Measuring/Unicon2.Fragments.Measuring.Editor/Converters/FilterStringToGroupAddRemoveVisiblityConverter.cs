using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Unicon2.Fragments.Measuring.Editor.Converters
{
	public class FilterStringToGroupAddRemoveVisibilityConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value?.ToString() == "Groups")
			{
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
