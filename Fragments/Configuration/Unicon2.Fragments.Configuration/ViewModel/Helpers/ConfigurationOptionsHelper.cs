using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
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

        private bool ExpandLevelByIndex(List<IRuntimeConfigurationItemViewModel> configurationItemViewModels,
            int requestedLevel, int currentLevel)
        {
            bool isExpanded = false;
            foreach (IRuntimeConfigurationItemViewModel configurationItemViewModel in configurationItemViewModels)
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

        private bool CollapseLevelByIndex(List<IRuntimeConfigurationItemViewModel> configurationItemViewModels,
            int requestedLevel, int currentLevel)
        {
            bool isCollapsed = false;
            foreach (IRuntimeConfigurationItemViewModel configurationItemViewModel in configurationItemViewModels)
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
            if (this.ExpandLevelByIndex(this._runtimeConfigurationViewModel.RootConfigurationItemViewModels.ToList(), this._levelIndex,
                0))
                this._levelIndex++;
            //  bool isLevelExpanded = false;
            //foreach (var rootConfigurationItemViewModel in _runtimeConfigurationViewModel
            //    .RootConfigurationItemViewModels)
            //{
            //    if (_levelIndex >= 0)
            //    {
            //        rootConfigurationItemViewModel.Checked?.Invoke(true);
            //        if (_levelIndex == 0) isLevelExpanded = true;
            //    }
            //    if (_levelIndex >= 1)
            //    {
            //        foreach (var configurationItemViewModel in rootConfigurationItemViewModel.ChildStructItemViewModels)
            //        {
            //            configurationItemViewModel.Checked?.Invoke(true);
            //            if (_levelIndex == 1) isLevelExpanded = true;
            //        }
            //    }
            //}
            //if (isLevelExpanded) _levelIndex++;
        }

        private void OnExecuteCollapseLevel()
        {
            if (this.CollapseLevelByIndex(this._runtimeConfigurationViewModel.RootConfigurationItemViewModels.ToList(),
                this._levelIndex, 0))
                this._levelIndex--;
            //bool isLevelCollapsed = false;
            //if (_levelIndex == 0) return;
            //foreach (var rootConfigurationItemViewModel in _runtimeConfigurationViewModel
            //    .RootConfigurationItemViewModels)
            //{
            //    if (_levelIndex <= 1)
            //    {
            //        rootConfigurationItemViewModel.Checked?.Invoke(false);
            //        if (_levelIndex == 1) isLevelCollapsed = true;
            //    }
            //    if (_levelIndex <=2)
            //    {
            //        foreach (var configurationItemViewModel in rootConfigurationItemViewModel.ChildStructItemViewModels)
            //        {
            //            configurationItemViewModel.Checked?.Invoke(false);
            //            if (_levelIndex ==2) isLevelCollapsed = true;
            //        }
            //    }
            //}

            //if (isLevelCollapsed) _levelIndex--;

        }

        private void OnExecuteLoadConfiguration()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                IDeviceConfiguration loadedConfig = this._container.Resolve<IDeviceConfiguration>();

                loadedConfig.DeserializeFromFile(ofd.FileName);
                if (!(this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).CheckEquality(loadedConfig)) return;

                foreach (IRuntimeConfigurationItemViewModel rootConfigurationItem in this._runtimeConfigurationViewModel.RootConfigurationItemViewModels)
                {
                    (rootConfigurationItem.Model as IConfigurationItem).InitializeLocalValue(
                        loadedConfig.RootConfigurationItemList[
                            this._runtimeConfigurationViewModel.RootConfigurationItemViewModels
                                .IndexOf(rootConfigurationItem)]);
                }
            }
        }

        private void OnExecuteSaveConfiguration()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".cnf";
            sfd.FileName = this._runtimeConfigurationViewModel.NameForUiKey;
            if (sfd.ShowDialog() == true)
            {
                (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).SerializeInFile(sfd.FileName, false);
            }
        }

        private async void OnExecuteWriteLocalValuesToDevice()
        {
            bool isWritten = await (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).Write();
            if (isWritten)
                (this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).FragmentSettings?.ApplySettingByKey(
                    ConfigurationKeys.Settings.ACTIVATION_CONFIGURATION_SETTING, null);
        }

        private void OnExecuteEditLocalValues()
        {
            //IDeviceConfiguration loadedConfig = this._container.Resolve<IDeviceConfiguration>();
            //if (!(this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).CheckEquality(loadedConfig)) return;
            //foreach (IRuntimeConfigurationItemViewModel rootConfigurationItem in this._runtimeConfigurationViewModel.RootConfigurationItemViewModels)
            //{
            //    (rootConfigurationItem.Model as IConfigurationItem).InitializeLocalValue(
            //        loadedConfig.RootConfigurationItemList[
            //            this._runtimeConfigurationViewModel.RootConfigurationItemViewModels
            //                .IndexOf(rootConfigurationItem)]);
            //}
            IDeviceConfiguration loadedConfig = this._container.Resolve<IDeviceConfiguration>();
            loadedConfig = _runtimeConfigurationViewModel.Model as IDeviceConfiguration;
            if (!(this._runtimeConfigurationViewModel.Model as IDeviceConfiguration).CheckEquality(loadedConfig)) return;

            try
            {
                foreach (IRuntimeConfigurationItemViewModel rootConfigurationItem in this._runtimeConfigurationViewModel.RootConfigurationItemViewModels)
                {

                    //(rootConfigurationItem.Model as IConfigurationItem).InitializeValue(rootConfigurationItem as IConfigurationItem);
                    (rootConfigurationItem.Model as IConfigurationItem).InitializeValue(

                        loadedConfig.RootConfigurationItemList[
                            this._runtimeConfigurationViewModel.RootConfigurationItemViewModels
                                .IndexOf(rootConfigurationItem)]);
                }
            }
            catch (Exception ex)
            { }
            //foreach (IRuntimeConfigurationItemViewModel rootConfigurationItem in this._runtimeConfigurationViewModel.RootConfigurationItemViewModels)
            //{

            //    //(rootConfigurationItem.Model as IConfigurationItem).InitializeValue(rootConfigurationItem as IConfigurationItem);
            //    (rootConfigurationItem.Model as IConfigurationItem).InitializeLocalValue(

            //        loadedConfig.RootConfigurationItemList[
            //            this._runtimeConfigurationViewModel.RootConfigurationItemViewModels
            //                .IndexOf(rootConfigurationItem)]);
            //}
        }

        private void OnExecuteTransferFromDeviceToLocal()
        {
            foreach (IRuntimeConfigurationItemViewModel rootConfigurationItem in this._runtimeConfigurationViewModel.RootConfigurationItemViewModels)
            {
                (rootConfigurationItem.Model as IConfigurationItem).TransferDeviceLocalData(true);
            }
        }
    }
}