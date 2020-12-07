using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.Fragments.Configuration.Converters
{
    public class BoolToBorderColorConverter : IValueConverter
    {
        public SolidColorBrush BrushIfTrue { get; set; }
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool?)
            {
                if (Invert)
                {
                    if ((value as bool?).Value)
                    {
                        return Brushes.Transparent;
                    }
                    else
                    {
                        return BrushIfTrue;

                    }
                }
                else
                {
                    if ((value as bool?).Value)
                    {
                        return BrushIfTrue;
                    }
                    else
                    {
                        return Brushes.Transparent;
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
