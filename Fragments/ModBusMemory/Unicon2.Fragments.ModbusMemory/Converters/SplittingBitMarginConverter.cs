using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Unicon2.Fragments.ModbusMemory.Converters
{
    public class SplittingBitMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if ((int) value == 8)
                {
                    return new Thickness(2, 2, 20, 2);
                }
                else
                {
                    return new Thickness(2);
                }
            }

            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}