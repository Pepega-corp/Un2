using System.Windows;
using System.Windows.Controls;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.SharedResources.LayoutHelpers
{
    public class FragmentsLayoutTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            //ResourceDictionary resourceDictionary = new ResourceDictionary();
            //    resourceDictionary.Source=new Uri("../Resources/DeviceStructDataTemplates.xaml");
            //получаем вызывающий контейнер
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is IStronglyNamed)
            {

                //  var t=element.TryFindResource((item as IStronglyNamed).StrongName +
                //                               ApplicationGlobalNames.CommonInjectionStrings
                //                                    .DATATEMPLATE) as DataTemplate;
                return element.TryFindResource((item as IStronglyNamed).StrongName +
                                               ApplicationGlobalNames.CommonInjectionStrings
                                                   .DATATEMPLATE) as DataTemplate;
            }

            return null;
        }
    }
}