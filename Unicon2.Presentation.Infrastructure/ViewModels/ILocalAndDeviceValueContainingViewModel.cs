using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface ILocalAndDeviceValueContainingViewModel
    {
        IFormattedValueViewModel LocalValue { get; set; }
        IFormattedValueViewModel DeviceValue { get; set; }
    }
}