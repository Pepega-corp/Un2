using System.Windows;
using System.Windows.Controls;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.SharedResources.LayoutHelpers
{
   public class DockingManagerTemplateSelector: DataTemplateSelector
    {
        public DataTemplate FragmentPaneTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IFragmentPaneViewModel)
            {
                return this.FragmentPaneTemplate;
            }
            return base.SelectTemplate(item,container);
        }
    }
}
