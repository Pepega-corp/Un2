using System.Windows;
using System.Windows.Controls;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.SharedResources.DataTemplateSelectors
{
    public class ViewModelByStrongNameDataTemplateSelector : DataTemplateSelector
    {

        #region Overrides of DataTemplateSelector

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IStronglyNamed)
            {
                FrameworkElement element = container as FrameworkElement;
                //var x =  element?.TryFindResource(((IStronglyNamed)item).StrongName + ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as DataTemplate;

                return element?.TryFindResource(((IStronglyNamed) item).StrongName + ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as DataTemplate;
            }
            return null;
        }

        #endregion
    }
}
