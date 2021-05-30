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
        public DataTemplate And { get; set; }
        public DataTemplate Or { get; set; }
        public DataTemplate Xor { get; set; }
        public DataTemplate System { get; set; }
        public DataTemplate Alarm { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case InputEditorViewModel _:
                    return this.Input;
                case OutputEditorViewModel _:
                    return this.Output;
                case InversionEditorViewModel _:
                    return this.Inversion;
                case AndEditorViewModel _:
                    return this.And;
                case OrEditorViewModel _:
                    return this.Or;
                case XorEditorViewModel _:
                    return this.Xor;
                case AlarmJournalEditorViewModel _:
                    return Alarm;
                case SystemJournalViewModel _:
                    return System;
                default:
                    return null;
            }
        }
    }
}
