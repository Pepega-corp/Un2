using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements
{
    public interface IAnalogMeasuringElementViewModel : IMeasuringElementViewModel, IMeasurable,IFormattedValueOwner
    {

    }
}