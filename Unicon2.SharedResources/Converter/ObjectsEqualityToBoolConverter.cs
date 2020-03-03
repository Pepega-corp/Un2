using System;
using System.Globalization;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class ObjectsEqualityToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == null) || (values[1] == null)) return true;
            if (values.Length != 2) return false;
            return values[0] == values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}