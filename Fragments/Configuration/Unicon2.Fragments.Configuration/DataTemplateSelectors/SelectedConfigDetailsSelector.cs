using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.Configuration.ViewModel;

namespace Unicon2.Fragments.Configuration.DataTemplateSelectors
{
    public class SelectedConfigDetailsSelector : DataTemplateSelector
    {

        public DataTemplate TableTemplate { get; set; }
        public DataTemplate ConfigTreeTemplate { get; set; }

        #region Overrides of DataTemplateSelector

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MainConfigItemViewModel mainConfigItem)
            {
                if (mainConfigItem.IsTableSelected)
                {
                    return TableTemplate;
                }

                return ConfigTreeTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        #endregion
    }
}