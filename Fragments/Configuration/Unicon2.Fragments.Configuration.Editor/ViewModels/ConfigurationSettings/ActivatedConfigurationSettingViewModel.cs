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
            this._activatedSettingItem = container.Resolve<IFragmentSetting>(ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING) as IActivatedSetting;
            this.ActivationAddress = "0";
        }

        public string StrongName => ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return this.SaveModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            this._activatedSettingItem = value as IActivatedSetting;

            if(this._activatedSettingItem == null) return;

            this.ActivationAddress = this._activatedSettingItem.ActivationAddress.ToString();
            this.IsSettingEnabled = this._activatedSettingItem.IsSettingEnabled;
        }

        private object SaveModel()
        {
            this._activatedSettingItem.ActivationAddress = ushort.Parse(this.ActivationAddress);
            this._activatedSettingItem.IsSettingEnabled = this.IsSettingEnabled;
            return this._activatedSettingItem;
        }

        public string ActivationAddress
        {
            get { return this._activationAddress; }
            set
            {
                this._activationAddress = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsSettingEnabled
        {
            get { return this._isSettingEnabled; }
            set
            {
                this._isSettingEnabled = value;
                this.RaisePropertyChanged();
            }
        }
    }
}