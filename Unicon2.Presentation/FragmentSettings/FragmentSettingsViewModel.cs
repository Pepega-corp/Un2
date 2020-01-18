using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Presentation.Infrastructure.FragmentSettings;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.FragmentSettings
{
    public class FragmentSettingsViewModel : ViewModelBase, IFragmentSettingsViewModel
    {
        private readonly ITypesContainer _container;
        private IFragmentSettings _configurationSettings;
        private List<IFragmentSetting> _availableConfigurationSettings;

        private string _selectedSettingToAdd;


        public FragmentSettingsViewModel(ITypesContainer container)
        {
            this._container = container;
            this.CancelCommand = new RelayCommand<object>(this.OnExecuteCancel);
            this.SubmitCommand = new RelayCommand<object>(this.OnExecuteSubmit);
            this.AddSelectedSettingCommand = new RelayCommand(this.OnAddSelectedSettingExecute);
            this._configurationSettings =
            this._container.Resolve<IFragmentSettings>();
        }

        private void OnAddSelectedSettingExecute()
        {
            if (this.SelectedSettingToAdd != null)
            {
                if (this._configurationSettings.FragmentSettings.Any(
                    (setting => setting.StrongName == this._selectedSettingToAdd))) ;
            }
        }

        private void OnExecuteSubmit(object obj)
        {
            this.SaveModel();
            (obj as Window)?.Close();
        }

        private void OnExecuteCancel(object obj)
        {
            (obj as Window)?.Close();
        }


        public string StrongName => nameof(FragmentSettingsViewModel);

        public object Model
        {
            get => this.SaveModel();
            set => this.SetModel(value);
        }

        private object SaveModel()
        {

            this._configurationSettings.FragmentSettings.Clear();
            foreach (IFragmentSettingViewModel configurationSettingViewModel in
                this.ConfigurationSettingViewModelCollection)
            {
                if (configurationSettingViewModel.IsSettingEnabled)
                {
                    this._configurationSettings.FragmentSettings.Add(
                        configurationSettingViewModel.Model as IFragmentSetting);
                }
            }
            return this._configurationSettings;
        }

        private void SetModel(object value)
        {
            if (value is IFragmentSettings)
            {
                this._configurationSettings = value as IFragmentSettings;
                this.ConfigurationSettingViewModelCollection.Clear();
                this._availableConfigurationSettings = new List<IFragmentSetting>(this._container.ResolveAll<IFragmentSetting>());
                foreach (IFragmentSetting configurationSetting in this._availableConfigurationSettings)
                {
                    IFragmentSettingViewModel configurationSettingViewModel =
                        this._container.Resolve<IFragmentSettingViewModel>(configurationSetting.StrongName +
                        ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                    this.ConfigurationSettingViewModelCollection.Add(configurationSettingViewModel);
                }
                foreach (IFragmentSetting configurationSetting in (value as IFragmentSettings)
                    .FragmentSettings)
                {
                    IFragmentSettingViewModel configurationSettingViewModel =
                        this.ConfigurationSettingViewModelCollection.FirstOrDefault(
                            (model => (model.Model as IFragmentSetting).StrongName == configurationSetting.StrongName));
                    if (configurationSettingViewModel != null)
                    {
                        configurationSettingViewModel.Model = configurationSetting;
                    }
                }
            }
        }


        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }


        public ObservableCollection<IFragmentSettingViewModel> ConfigurationSettingViewModelCollection { get; } = new ObservableCollection<IFragmentSettingViewModel>();


        public string SelectedSettingToAdd
        {
            get { return this._selectedSettingToAdd; }
            set
            {
                this._selectedSettingToAdd = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand AddSelectedSettingCommand { get; }
    }
}