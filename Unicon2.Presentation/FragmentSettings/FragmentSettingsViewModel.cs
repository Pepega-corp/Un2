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
            _container = container;
            CancelCommand = new RelayCommand<object>(OnExecuteCancel);
            SubmitCommand = new RelayCommand<object>(OnExecuteSubmit);
            AddSelectedSettingCommand = new RelayCommand(OnAddSelectedSettingExecute);
            _configurationSettings =
            _container.Resolve<IFragmentSettings>();
        }

        private void OnAddSelectedSettingExecute()
        {
            if (SelectedSettingToAdd != null)
            {
                if (_configurationSettings.FragmentSettings.Any(
                    (setting => setting.StrongName == _selectedSettingToAdd))) ;
            }
        }

        private void OnExecuteSubmit(object obj)
        {
            SaveModel();
            (obj as Window)?.Close();
        }

        private void OnExecuteCancel(object obj)
        {
            (obj as Window)?.Close();
        }


        public string StrongName => nameof(FragmentSettingsViewModel);

        public object Model
        {
            get => SaveModel();
            set => SetModel(value);
        }

        private object SaveModel()
        {

            _configurationSettings.FragmentSettings.Clear();
            foreach (IFragmentSettingViewModel configurationSettingViewModel in
                ConfigurationSettingViewModelCollection)
            {
                if (configurationSettingViewModel.IsSettingEnabled)
                {
                    _configurationSettings.FragmentSettings.Add(
                        configurationSettingViewModel.Model as IFragmentSetting);
                }
            }
            return _configurationSettings;
        }

        private void SetModel(object value)
        {
            if (value == null)
            {
                value = _container.Resolve<IFragmentSettings>();
            }
            if (value is IFragmentSettings)
            {
                _configurationSettings = value as IFragmentSettings;
                ConfigurationSettingViewModelCollection.Clear();
                _availableConfigurationSettings = new List<IFragmentSetting>(_container.ResolveAll<IFragmentSetting>());
                foreach (IFragmentSetting configurationSetting in _availableConfigurationSettings)
                {
                    IFragmentSettingViewModel configurationSettingViewModel =
                        _container.Resolve<IFragmentSettingViewModel>(configurationSetting.StrongName +
                        ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                    ConfigurationSettingViewModelCollection.Add(configurationSettingViewModel);
                }
                foreach (IFragmentSetting configurationSetting in (value as IFragmentSettings)
                    .FragmentSettings)
                {
                    IFragmentSettingViewModel configurationSettingViewModel =
                        ConfigurationSettingViewModelCollection.FirstOrDefault(
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
            get { return _selectedSettingToAdd; }
            set
            {
                _selectedSettingToAdd = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AddSelectedSettingCommand { get; }
    }
}