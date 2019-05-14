using System;
using System.Globalization;
using System.Windows.Data;

namespace Oscilloscope.View.PieChartItem
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value).ToString(CultureInfo.CurrentCulture);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(value.ToString(), CultureInfo.CurrentCulture);
        }
    }
}
