using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IDependentPropertyEditorViewModel : IDependentPropertyViewModel, IPropertyEditorViewModel
    {
        ICommand SubmitCommand { get; }
        ICommand CancelCommand { get; }

        ICommand AddConditionCommand { get; }
        ICommand DeleteConditionCommand { get; }
        IConditionViewModel SelectedConditionViewModel { get; set; }
        ObservableCollection<IConditionViewModel> ConditionViewModels { get; }
    }
}