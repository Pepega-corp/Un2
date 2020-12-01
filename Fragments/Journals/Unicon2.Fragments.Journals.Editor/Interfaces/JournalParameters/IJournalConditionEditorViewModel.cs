using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters
{
    public interface IJournalConditionEditorViewModel : IViewModel, IEditable, IUshortFormattableEditorViewModel
    {
        void SetAvailablePatameters(List<IJournalParameter> availableJournalParameters);
        List<string> AvailableJournalParameters { get; }
        string SelectedJournalParameter { get; set; }

        List<string> ConditionsList { get; set; }
        string SelectedCondition { get; set; }
        ushort UshortValueToCompare { get; set; }
        string UshortFormatterString { get; }
        ICommand ShowFormatterParameters { get; }
    }
}