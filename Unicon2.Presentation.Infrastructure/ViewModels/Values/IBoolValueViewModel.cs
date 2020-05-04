using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IBoolValueViewModel : IFormattedValueViewModel
    {
        bool BoolValueProperty { get; set; }
        void SetBoolValueProperty(bool value);

    }
}