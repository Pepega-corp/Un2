using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.SharedResources.DataTemplateSelectors
{
    public class TableViewByNameDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IAsTableViewModel)
            {
                FrameworkElement element = container as FrameworkElement;
                //var x =  element?.TryFindResource(((IStronglyNamed)item).StrongName + ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as DataTemplate;

                return element?.TryFindResource(((IAsTableViewModel) item).AsossiatedDetailsViewName +
                                                ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as
                    DataTemplate;
            }

            return null;
        }
    }
}