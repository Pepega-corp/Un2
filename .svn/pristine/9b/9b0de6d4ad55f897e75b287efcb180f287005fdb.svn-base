using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.Converters
{
    public class IsFormattableToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IUshortFormattableEditorViewModel)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}