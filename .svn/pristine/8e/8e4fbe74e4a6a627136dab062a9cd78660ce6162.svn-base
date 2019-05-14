using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class IsInterfaceImplementedToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            string interfaceString = parameter.ToString();

            // var interfaces = value.GetType().GetInterfaces();
            return value.GetType().GetInterface(interfaceString) != null ? Visibility.Visible : Visibility.Collapsed;



            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
