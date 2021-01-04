using System.Windows;
using System.Windows.Controls;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.ViewModels.Windows;

namespace Unicon2.Presentation.DataTemplateSelectors
{
    public class LoadFragmentViewModelDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate DefaultLoadFragmentDataTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DefaultLoadFragmentViewModel)
            {
                return DefaultLoadFragmentDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}