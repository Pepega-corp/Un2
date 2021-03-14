using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Unicon2.SharedResources.Converter
{
    public class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var r=new BitmapImage(new Uri("/Error.png", UriKind.Relative));
            bool isSuccess;
            if (value is bool)
            {
                isSuccess = (bool)value;
                if (!isSuccess) return new BitmapImage(new Uri("/Unicon2.SharedResources;component/Icons/Error.png", UriKind.Relative));
                else
                {
                    return new BitmapImage(new Uri("/Unicon2.SharedResources;component/Icons/ExchangeSuccess.png", UriKind.Relative));
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
