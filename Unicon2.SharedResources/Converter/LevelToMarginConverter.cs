using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    /// <summary>
    /// Convert Level to left margin
    /// </summary>
    public class LevelToMarginConverter : IValueConverter
    {
        private const double IndentSize = 20.0;

        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            return new Thickness((int)o *IndentSize, 0, 3, 0);
        }

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
