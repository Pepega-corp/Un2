using System;
using System.Globalization;
using System.Windows.Data;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.Converters
{
    public class PropertyTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IFormattedValueViewModel formattedValueViewModel)
            {
                string toolTip=String.Empty;
                if (formattedValueViewModel.IsMeasureUnitEnabled)
                {
                    toolTip += formattedValueViewModel.MeasureUnit;
                }
                if (formattedValueViewModel.IsRangeEnabled)
                {
                    if (toolTip != string.Empty)
                    {
                        toolTip += "\n";
                    }
                    toolTip += $"[{formattedValueViewModel.Range.RangeFrom}:{formattedValueViewModel.Range.RangeTo}]";
                }

                if (formattedValueViewModel.IsMeasureUnitEnabled || formattedValueViewModel.IsRangeEnabled)
                {
                    return toolTip;
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
