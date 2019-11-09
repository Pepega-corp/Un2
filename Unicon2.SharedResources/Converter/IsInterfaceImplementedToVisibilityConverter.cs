using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class IsInterfaceImplementedToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public bool IsInverted { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            string interfaceString = parameter.ToString();

            // var interfaces = value.GetType().GetInterfaces();
            var ifImplementedVisibility = IsInverted ? Visibility.Collapsed : Visibility.Visible;
            var ifNotImplementedVisibility = IsInverted ? Visibility.Visible : Visibility.Collapsed;

            return value.GetType().GetInterface(interfaceString) != null ? ifImplementedVisibility : ifNotImplementedVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
