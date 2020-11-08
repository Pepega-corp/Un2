using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IToolBarService
    {
        void SetCurrentFragmentToolbar(IFragmentOptionsViewModel fragmentOptionsViewModel);
    }
}
