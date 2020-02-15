using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.Icons;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Helpers
{
    public class ConfigurationOptionsHelper
    {
        private IRuntimeConfigurationViewModel _runtimeConfigurationViewModel;
        private ITypesContainer _container;
        private int _levelIndex = 0;

        public IFragmentOptionsViewModel CreateConfigurationFragmentOptionsViewModel(IRuntimeConfigurationViewModel runtimeConfigurationViewModel, ITypesContainer container)
        {
            this._runtimeConfigurationViewModel = runtimeConfigurationViewModel;
            this._container = container;
            
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelGettingFunc =
                container.Resolve<Func<IFragmentOptionGroupViewModel>>();
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelGettingFunc =
                container.Resolve<Func<IFragmentOptionCommandViewModel>>();

            IFragmentOptionsViewModel fragmentOptionsViewModel = container.Resolve<IFragmentOptionsViewModel>();


            fragmentOptionsViewModel.FragmentOptionGroupViewModels = new ObservableCollection<IFragmentOptionGroupViewModel>();


            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel =
                fragmentOptionGroupViewModelGettingFunc();




            //группа устройство
            fragmentOptionGroupViewModel.NameKey = ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();

            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel =
                fragmentOptionCommandViewModelGettingFunc();


            fragmentOptionCommandViewModel.TitleKey = ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxIn;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteReadConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconChevronRight;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteTransferFromDeviceToLocal);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxOut;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteWriteLocalValuesToDevice);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.EDIT_LOCAL_CONFIGURATION_VALUES_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconEdit;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteEditLocalValues);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);
            fragmentOptionGroupViewModel = fragmentOptionGroupViewModelGettingFunc();
            //группа файл
            fragmentOptionGroupViewModel.NameKey = ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.LOAD_FROM_FILE_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscUpload;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteLoadConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.SAVE_CONFUGURATION_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscDownload;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteSaveConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.EXPORT_CONFUGURATION_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconPrintText;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteExportConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);



            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);

            // группа дерево
            fragmentOptionGroupViewModel = fragmentOptionGroupViewModelGettingFunc();
            fragmentOptionGroupViewModel.NameKey = ApplicationGlobalNames.UiGroupingStrings.TREE_STRING_KEY;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.EXPAND_LEVEL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconStepInto;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteExpandLevel);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);


            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.COLLAPSE_LEVEL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconStepOut;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(this.OnExecuteCollapseLevel);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);


            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);

            return fragmentOptionsViewModel;
        }

        private void OnExecuteReadConfiguration()
        {
            (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).Load();
        }

        private bool ExpandLevelByIndex(List<IConfigurationItemViewModel> configurationItemViewModels,
            int requestedLevel, int currentLevel)
        {
            bool isExpanded = false;
            foreach (IConfigurationItemViewModel configurationItemViewModel in configurationItemViewModels)
            {
                if (currentLevel == requestedLevel)
                {
                    if ((!configurationItemViewModel.IsChecked) &&
                        (configurationItemViewModel.ChildStructItemViewModels.Count != 0))
                    {
                        configurationItemViewModel.Checked?.Invoke(true);
                        isExpanded = true;
                    }
                    if (configurationItemViewModel.IsChecked) isExpanded = true;
                }
                if (currentLevel < requestedLevel)
                {
                    if ((!configurationItemViewModel.IsChecked) && (configurationItemViewModel.IsCheckable))
                    {
                        configurationItemViewModel.Checked?.Invoke(true);
                    }
                }
                if (this.ExpandLevelByIndex(configurationItemViewModel.ChildStructItemViewModels.ToList(),
                    requestedLevel,
                    currentLevel + 1)) isExpanded = true;
            }
            return isExpanded;
        }

        private bool CollapseLevelByIndex(List<IConfigurationItemViewModel> configurationItemViewModels,
            int requestedLevel, int currentLevel)
        {
            bool isCollapsed = false;
            foreach (IConfigurationItemViewModel configurationItemViewModel in configurationItemViewModels)
            {
                if (this.CollapseLevelByIndex(configurationItemViewModel.ChildStructItemViewModels.ToList(),
                    requestedLevel,
                    currentLevel + 1))
                    isCollapsed = true;
                if (currentLevel + 1 == requestedLevel)
                {
                    if (configurationItemViewModel.IsChecked)
                    {
                        configurationItemViewModel.Checked?.Invoke(false);
                        isCollapsed = true;
                    }
                    if (!configurationItemViewModel.IsChecked) isCollapsed = true;
                }
                if (currentLevel + 1 > requestedLevel)
                {
                    configurationItemViewModel.Checked?.Invoke(false);
                }
            }
            return isCollapsed;
        }

        private void OnExecuteExpandLevel()
        {
            if (this.ExpandLevelByIndex(this._runtimeConfigurationViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>().ToList(), this._levelIndex,
                0))
                this._levelIndex++;
        }

        private void OnExecuteCollapseLevel()
        {
            if (this.CollapseLevelByIndex(this._runtimeConfigurationViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>().ToList(),
                this._levelIndex, 0))
                this._levelIndex--;

        }

        private void OnExecuteLoadConfiguration()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                IConfigurationMemory loadedConfigMemory = _container.Resolve<ISerializerService>()
                    .DeserializeFromFile<IConfigurationMemory>(ofd.FileName);
                (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).ConfigurationMemory =
                    loadedConfigMemory;
            }
        }
        private void OnExecuteExportConfiguration()
        {
            ConfigurationExportHelper.ExportConfiguration(this._runtimeConfigurationViewModel.Model as IDeviceConfiguration,_container,_runtimeConfigurationViewModel.GetDeviceName(), _runtimeConfigurationViewModel.NameForUiKey);
        }
        private void OnExecuteSaveConfiguration()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".cnf";
            sfd.FileName = this._runtimeConfigurationViewModel.NameForUiKey;
            if (sfd.ShowDialog() == true)
            {
                _container.Resolve<ISerializerService>().SerializeInFile((this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).ConfigurationMemory,sfd.FileName);
            }
        }

        private async void OnExecuteWriteLocalValuesToDevice()
        {
            bool isWritten = await (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).Write();
            if (isWritten)
                (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).FragmentSettings?.ApplySettingByKey(
                    ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING, null);
        }

        private async void OnExecuteEditLocalValues()
        {
            await (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).InitializeLocalValues();
        }

        private async void OnExecuteTransferFromDeviceToLocal()
        {
            await(this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).TransferLocalToDeviceValues();
            
        }
    }
}