using Unicon2.Fragments.Configuration.Editor.Interfaces.ConfigurationSettings;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.ConfigurationSettings
{
    public class ActivatedConfigurationSettingViewModel : ViewModelBase, IActivatedConfigurationSettingViewModel
    {
        private IActivatedSetting _activatedSettingItem;
        private string _activationAddress;
        private bool _isSettingEnabled;

        public ActivatedConfigurationSettingViewModel(ITypesContainer container)
        {
            _activatedSettingItem = container.Resolve<IFragmentSetting>(ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING) as IActivatedSetting;
            ActivationAddress = "0";
        }

        public string StrongName => ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return SaveModel(); }
            set { SetModel(value); }
        }

        private void SetModel(object value)
        {
            _activatedSettingItem = value as IActivatedSetting;

            if(_activatedSettingItem == null) return;

            ActivationAddress = _activatedSettingItem.ActivationAddress.ToString();
            IsSettingEnabled = _activatedSettingItem.IsSettingEnabled;
        }

        private object SaveModel()
        {
            _activatedSettingItem.ActivationAddress = ushort.Parse(ActivationAddress);
            _activatedSettingItem.IsSettingEnabled = IsSettingEnabled;
            return _activatedSettingItem;
        }

        public string ActivationAddress
        {
            get { return _activationAddress; }
            set
            {
                _activationAddress = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSettingEnabled
        {
            get { return _isSettingEnabled; }
            set
            {
                _isSettingEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}