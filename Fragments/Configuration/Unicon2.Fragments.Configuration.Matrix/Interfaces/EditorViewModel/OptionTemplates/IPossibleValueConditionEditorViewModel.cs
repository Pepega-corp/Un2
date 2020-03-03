using System.Collections.Generic;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates
{
    public interface IPossibleValueConditionEditorViewModel : IViewModel
    {
        bool BoolConditionRule { get; set; }
        IOptionPossibleValueEditorViewModel RelatedOptionPossibleValueEditorViewModel { get; set; }

        void SetAvailableOptionPossibleValueEditorViewModel(
            List<IOptionPossibleValueEditorViewModel> availableOptionPossibleValueEditorViewModels);
    }
}