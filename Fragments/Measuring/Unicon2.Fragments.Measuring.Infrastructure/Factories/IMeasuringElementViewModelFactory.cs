using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;

namespace Unicon2.Fragments.Measuring.Infrastructure.Factories
{
    public interface IMeasuringElementViewModelFactory
    {
        IMeasuringElementViewModel CreateMeasuringElementViewModel(IMeasuringElement measuringElement,string groupName);
    }
}