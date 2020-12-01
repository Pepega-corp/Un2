using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IFormatterViewModelFactory
    {
        IFormatterParametersViewModel CreateFormatterViewModel(IUshortsFormatter ushortsFormatter);
    }
}