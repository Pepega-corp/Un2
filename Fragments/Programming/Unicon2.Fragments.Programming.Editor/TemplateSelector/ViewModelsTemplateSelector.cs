using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels;

namespace Unicon2.Fragments.Programming.Editor.TemplateSelector
{
    public class ViewModelsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Input { get; set; }
        public DataTemplate Output { get; set; }
        public DataTemplate Inversion { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is InputEditorViewModel)
                return this.Input;
            if (item is OutputEditorViewModel)
                return this.Output;
            if (item is InversionEditorViewModel)
                return this.Inversion;
            return null;
        }
    }
}
