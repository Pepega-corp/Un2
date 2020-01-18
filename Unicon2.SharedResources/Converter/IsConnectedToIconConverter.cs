using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Unicon2.SharedResources.Converter
{
    class IsConnectedToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var r=new BitmapImage(new Uri("/Error.png", UriKind.Relative));
            bool isConnected;
            if (value is bool)
            {
                isConnected = (bool)value;
                if (!isConnected) return new BitmapImage(new Uri("../Icons/Error.png", UriKind.Relative));
                else
                {
                    return new BitmapImage(new Uri("../Icons/ExchangeSuccess.png", UriKind.Relative));
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
