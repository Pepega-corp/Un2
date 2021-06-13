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
        public DataTemplate BooleanTemplate { get; set; }
        public DataTemplate Timer { get; set; }
        public DataTemplate Trigger { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case InputEditorViewModel _:
                    return this.Input;
                case OutputEditorViewModel _:
                case AlarmJournalEditorViewModel _:
                case SystemJournalViewModel _:
                    return this.Output;
                case InversionEditorViewModel _:
                    return this.Inversion;
                case AndEditorViewModel _:
                case OrEditorViewModel _:
                case XorEditorViewModel _:
                    return this.BooleanTemplate;
                case TimerEditorViewModel _:
                    return this.Timer;
                case RsTriggerEditorViewModel _:
                case SrTriggerEditorViewModel _:
                    return this.Trigger;
                default:
                    return null;
            }
        }
    }
}
