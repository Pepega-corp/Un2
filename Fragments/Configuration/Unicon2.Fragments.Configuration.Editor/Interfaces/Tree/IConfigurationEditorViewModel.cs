using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IConfigurationEditorViewModel : IConfigurationViewModel, IFragmentEditorViewModel,
        IChildItemRemovable
    {
        IEditorConfigurationItemViewModel SelectedRow { get; set; }
        ICommand AddRootGroupElementCommand { get; set; }
        ICommand AddRootElementCommand { get; set; }
        ICommand EditElementCommand { get; set; }
        ICommand DeleteElementCommand { get; set; }
        ICommand ShowFormatterParametersCommand { get; set; }
        ICommand SetElementUpCommand { get; set; }
        ICommand SetElementDownCommand { get; set; }
        ICommand OpenConfigurationSettingsCommand { get; set; }
        ICommand CopyElementCommand { get; }
        ICommand PasteAsChildElementCommand { get; }
        ICommand AddSelectedElementAsResourceCommand { get; }
        ICommand EditDescriptionCommand { get; }
        ObservableCollection<IElementAddingCommand> ElementsAddingCommandCollection { get; set; }
    }
}