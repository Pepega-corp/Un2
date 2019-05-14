using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;

namespace Unicon2.DeviceEditorUtilityModule.Interfaces
{
    public interface IConnectionStateViewModel:IViewModel
    {
        ObservableCollection<StringWrapper> ExpectedValues { get;  }
        IComPortConfigurationViewModel DefaultComPortConfigurationViewModel { get; }
        string SelectedPropertyString { get;}
        ICommand SelectTestConnectionProperty { get;}
        ICommand SubmitCommand { get; }
        ICommand CancelCommand { get; }
        ICommand DeleteExpectedValueCommand { get; }
        ICommand AddExpectedValueCommand { get; }
    }
}