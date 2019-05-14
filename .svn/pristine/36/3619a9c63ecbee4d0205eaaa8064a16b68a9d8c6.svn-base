using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Converters
{
   public class IsItemsGroupToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IItemGroupViewModel)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
