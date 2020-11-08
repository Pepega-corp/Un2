using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Unicon2.Fragments.Measuring.Converters
{
   public class CommandSuccessToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Brushes.Transparent;
            }
            if (value.Equals(true))
            {
                return Brushes.Green;
            }
            if (value.Equals(false))
            {
                return Brushes.Crimson;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
