using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters
{
    public interface IJournalParameterEditorViewModel:IMeasurable,IEditable,IViewModel,IUshortFormattableEditorViewModel
    {
        string Header { get; set; }
        string Address { get; set; }
        string NumberOfPoints { get; set; }
        string FormatterString { get;}
    }
}