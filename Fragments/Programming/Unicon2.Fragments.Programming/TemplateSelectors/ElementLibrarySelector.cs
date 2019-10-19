using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;

namespace Unicon2.Fragments.Programming.TemplateSelectors
{
    public class ElementLibrarySelector : DataTemplateSelector
    {
        public DataTemplate Input { get; set; }
        public DataTemplate Output { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is InputViewModel)
                return this.Input;
            if (item is OutputViewModel)
                return this.Output;
            return null;
        }
    }
}