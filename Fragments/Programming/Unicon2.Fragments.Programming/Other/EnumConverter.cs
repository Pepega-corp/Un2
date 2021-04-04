using System;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Unicon2.Fragments.Programming.Other
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            foreach (var one in Enum.GetValues(parameter as Type))
            {
                if (value.Equals(one))
                {
                    var fieldInfo = value.GetType().GetField(value.ToString());
                    var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                    if (attributes != null && attributes.Any())
                    {
                        return attributes.First().Description;
                    }

                    return value.ToString();
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            foreach (var one in Enum.GetValues(parameter as Type))
            {
                var fieldInfo = one.GetType().GetField(one.ToString());
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any() && value.ToString() == attributes.First().Description)
                {
                    return one;
                }
            }

            return null;
        }
    }
}
