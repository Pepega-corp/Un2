using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Unicon2.Fragments.Configuration.ViewModel.Table;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Converters
{
    public class CellToBorderColorConverter : IValueConverter
    {
        private static SolidColorBrush _solidColorBrush =
            new SolidColorBrush((Color) ColorConverter.ConvertFromString("#72D3D3D3"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridCell dataGridCell)
            {
                if (dataGridCell.DataContext is List<ILocalAndDeviceValueContainingViewModel>
                    localAndDeviceValueContainingViewModels)
                {

                    var x = localAndDeviceValueContainingViewModels[dataGridCell.Column.DisplayIndex];
                    if (x is ConfigItemWrapper configItemWrapper)
                    {
                        if (!configItemWrapper.MatchesFilter)
                        {
                            return _solidColorBrush;
                        }
                    }
                }
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}