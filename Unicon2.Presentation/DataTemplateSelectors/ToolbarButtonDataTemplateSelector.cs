using System.Windows;
using System.Windows.Controls;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.ViewModels.Fragment.FragmentOptions;

namespace Unicon2.Presentation.DataTemplateSelectors
{
    public class ToolbarButtonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ToggleButtonDataTemplate { get; set; }
        public DataTemplate DefaultButtonDataTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is FragmentOptionToggleCommandViewModel)
            {
                return ToggleButtonDataTemplate;
            }

            if (item is DefaultFragmentOptionCommandViewModel)
            {
                return DefaultButtonDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}