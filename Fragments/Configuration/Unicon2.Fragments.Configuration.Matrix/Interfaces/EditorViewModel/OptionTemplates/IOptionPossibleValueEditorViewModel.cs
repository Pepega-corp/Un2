using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates
{
    public interface IOptionPossibleValueEditorViewModel : IViewModel
    {
        string PossibleValueName { get; set; }
        ObservableCollection<IPossibleValueConditionEditorViewModel> PossibleValueConditionEditorViewModels { get; }
        ICommand AddConditionCommand { get; }
        ICommand ResetConditionsCommand { get; }

        void SetAvailableOptionPossibleValues(
            List<IOptionPossibleValueEditorViewModel> optionPossibleValueEditorViewModels);

    }
}