using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;

namespace Unicon2.Fragments.Programming.TemplateSelectors
{
    public class ElementsViewModelSelector : DataTemplateSelector
    {
        public DataTemplate Input { get; set; }
        public DataTemplate Output { get; set; }
        public DataTemplate Inversion { get; set; }
        public DataTemplate Timer { get; set; }
        public DataTemplate SimpleLogic { get; set; }
        public DataTemplate Trigger { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case InputViewModel _:
                    return this.Input;
                case OutputViewModel _:
                case SystemJournalViewModel _:
                case AlarmJournalViewModel _:
                    return this.Output;
                case InversionViewModel _:
                    return this.Inversion;
                case SimpleLogicElementViewModel _:
                    return this.SimpleLogic;
                case TimerViewModel _:
                    return this.Timer;
                case RsTriggerViewModel _:
                case SrTriggerViewModel _:
                    return Trigger;
            }
            return null;
        }
    }
}