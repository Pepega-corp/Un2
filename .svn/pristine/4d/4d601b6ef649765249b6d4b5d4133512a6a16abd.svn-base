using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface IFormulaFormatterViewModel : IDynamicFormatterViewModel
    {
        string FormulaString { get; set; }
        double TestValueOfX { get; set; }
        string TestResult { get; set; }
        ICommand CheckCommand { get; set; }
        ObservableCollection<IArgumentViewModel> ArgumentViewModels { get; }
        ICommand DeleteArgumentCommand { get; }
        ICommand AddArgumentCommand { get; }
        ushort NumberOfSimbolsAfterComma { get; set; }
    }
}