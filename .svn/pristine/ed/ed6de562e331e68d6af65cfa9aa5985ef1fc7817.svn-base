using System;
using System.Globalization;
using System.Windows.Data;

namespace Oscilloscope.View.MainItem
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = ((double)value).ToString(CultureInfo.CurrentCulture);
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ret;
            double.TryParse(value.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out ret);
            return ret;
        }
    }
}
