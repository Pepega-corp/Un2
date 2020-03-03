using System;
using System.Globalization;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class IsInterfaceImplementedToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            string interfaceString = parameter.ToString();
            // var interfaces = value.GetType().GetInterfaces();
            return value.GetType().GetInterface(interfaceString) != null;



            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}