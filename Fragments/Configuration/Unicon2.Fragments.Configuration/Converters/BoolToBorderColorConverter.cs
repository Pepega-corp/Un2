using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Unicon2.Fragments.Configuration.Converters
{
    public class BoolToBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
       {
            if (value is bool?)
            {
                if ((value as bool?).Value)
                {
                    return Brushes.Yellow;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
