using Unicon2.Presentation.Infrastructure.FragmentSettings;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.ConfigurationSettings
{
    public interface IActivatedConfigurationSettingViewModel : IFragmentSettingViewModel
    {
        string ActivationAddress { get; set; }
    }
}