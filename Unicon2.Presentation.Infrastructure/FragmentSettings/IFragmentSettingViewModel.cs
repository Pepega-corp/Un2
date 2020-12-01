using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.FragmentSettings
{
    public interface IFragmentSettingViewModel : IViewModel
    {
        bool IsSettingEnabled { get; set; }

    }
}