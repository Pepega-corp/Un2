using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Shell.TemplateSelectors
{
    public class MainMenuItemTemplateSelector: DataTemplateSelector
    {
        public DataTemplate GroupedItemDataTemplate { get; set; }

        public DataTemplate MenuItemDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is GroupMenuItemViewModel)
            {
                return GroupedItemDataTemplate;
            }

            if (item is MenuItemViewModel)
            {
                return MenuItemDataTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
