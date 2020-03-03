using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Unicon2.ModuleDeviceEditing.Interfaces;

namespace Unicon2.ModuleDeviceEditing.Converters
{
    public class CurrentModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as ModesEnum?) == null) return Visibility.Visible;
            if (!(value as ModesEnum?).HasValue) return Visibility.Visible;
            return (value as ModesEnum?).Value == ModesEnum.AddingMode ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}