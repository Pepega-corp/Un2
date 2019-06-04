using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Connection
{
    public interface IConnectionStateViewModel : IViewModel
    {
        bool IsDeviceConnected { get; }
        double IndicatorOpacity { get; set; }
        IFormattedValueViewModel TestValueViewModel { get; set; }
        ICommand CheckConnectionCommand { get; }
    }
}
