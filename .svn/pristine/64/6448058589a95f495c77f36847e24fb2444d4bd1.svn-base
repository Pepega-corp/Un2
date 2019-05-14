using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
   public class ObjectEqualityDescriptionNullToVisibilityConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == null) || (values[1] == null)) return Visibility.Collapsed;
            if (values.Length != 3) return Visibility.Collapsed;
            if((values[0] == values[1])&&(values[2]!=null))return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
