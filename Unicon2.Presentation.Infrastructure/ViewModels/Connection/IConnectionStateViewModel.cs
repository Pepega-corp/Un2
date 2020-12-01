using System.Threading.Tasks;
using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Connection
{
    public interface IConnectionStateViewModel 
    {
        bool IsDeviceConnected { get; set; }
        double IndicatorOpacity { get; set; }
        string TestValue { get; set; }
        ICommand CheckConnectionCommand { get; }
        Task BeginIndication();
    }
}
