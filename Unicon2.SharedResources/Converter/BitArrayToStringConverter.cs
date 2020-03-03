using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Unicon2.SharedResources.Converter
{
    public class BitArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (value is List<bool> bitArray)
            {
                foreach (var bit in bitArray)
                {
                    stringBuilder.Append(bit ? '1' : '0');
                }
            }

            return stringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}