using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Formatting.Editor.Convertors
{
   public class ResourceStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var notSet = StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString("NotSet");
            if (value is string resourceString)
            {
                if (resourceString == null)
                {
                    return notSet;
                }

                return resourceString;
            }

            return notSet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
