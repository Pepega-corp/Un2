using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Unicon2.SharedResources.Converter
{
    public class BoolToVisConverter : MarkupExtension, IValueConverter

    {

        public bool Invert { get; set; }
        public Visibility? AlternateVisibility { get; set; }


        public override object ProvideValue(IServiceProvider serviceProvider)

        {

            return this;

        }



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)

        {

            bool isVisible = true.Equals(value);

            if (Invert) isVisible = !isVisible;


           //var t= isVisible ? Visibility.Visible : AlternateVisibility ?? Visibility.Collapsed;
            return isVisible ? Visibility.Visible :AlternateVisibility ?? Visibility.Collapsed;

        }



        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)

        {

            bool isVisible = Visibility.Visible.Equals(value);

            if (Invert) isVisible = !isVisible;

            return isVisible;

        }

    }
}
