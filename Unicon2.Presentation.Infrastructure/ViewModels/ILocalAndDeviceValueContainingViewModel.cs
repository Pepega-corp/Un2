using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface ILocalAndDeviceValueContainingViewModel
    {
        IEditableValueViewModel<> LocalValue { get; set; }
        IFormattedValueViewModel DeviceValue { get; set; }
    }
}