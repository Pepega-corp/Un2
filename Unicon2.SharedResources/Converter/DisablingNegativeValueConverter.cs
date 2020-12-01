using System;
using System.Globalization;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class DisablingNegativeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val;
            if (value == null)
            {
                return 0;
            }

            if (int.TryParse(value.ToString(), out val))
            {
                if (val < 0) return 0;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}