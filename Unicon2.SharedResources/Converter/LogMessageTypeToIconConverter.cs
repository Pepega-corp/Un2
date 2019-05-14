using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Unicon2.Infrastructure.Services.LogService;

namespace Unicon2.SharedResources.Converter
{
   public class LogMessageTypeToIconConverter:IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var r=new BitmapImage(new Uri("/Error.png", UriKind.Relative));
            LogMessageTypeEnum logMessageType;
            if (value is LogMessageTypeEnum)
            {
                logMessageType = (LogMessageTypeEnum) value;
                switch (logMessageType)
                {
                    case LogMessageTypeEnum.Error: return new BitmapImage(new Uri("../Icons/Error.png", UriKind.Relative));
                    case LogMessageTypeEnum.FailedQuery: return new BitmapImage(new Uri("../Icons/ExchangeFail.png", UriKind.Relative));
                    case LogMessageTypeEnum.Info: return new BitmapImage(new Uri("../Icons/Info.png", UriKind.Relative));
                    case LogMessageTypeEnum.SuccsessfulQuery: return new BitmapImage(new Uri("../Icons/ExchangeSuccess.png", UriKind.Relative));
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
