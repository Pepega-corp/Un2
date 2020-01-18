using System.Windows;
using System.Windows.Controls;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.TemplateSelectors
{
    public class FragmentsEditorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //получаем вызывающий контейнер
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item is IStronglyNamed)
            {
                DataTemplate template = element.TryFindResource(((IStronglyNamed)item).StrongName + ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as DataTemplate;
                return template;
            }
            return null;
        }
    }
}