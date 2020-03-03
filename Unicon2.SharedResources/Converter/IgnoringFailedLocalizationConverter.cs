using System;
using System.Globalization;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class IgnoringFailedLocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if (((string) value).Contains("Key:"))
                {
                    return ((string) value).Replace("Key:", "");
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}