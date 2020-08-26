using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Subscription;
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
        private IDeviceConfiguration _deviceConfiguration;
        private bool _isQueryInProgress = false;

        private RelayCommand ReadConfigurationCommand;
        private RelayCommand WriteConfigurationCommand;


        private void SetQueriesLock(bool isLocked)
        {
            this._isQueryInProgress = isLocked;
            this.ReadConfigurationCommand.RaiseCanExecuteChanged();
            this.WriteConfigurationCommand.RaiseCanExecuteChanged();

        }

        public IFragmentOptionsViewModel CreateConfigurationFragmentOptionsViewModel(
            IRuntimeConfigurationViewModel runtimeConfigurationViewModel, ITypesContainer container,
            IDeviceConfiguration deviceConfiguration)
        {
            _runtimeConfigurationViewModel = runtimeConfigurationViewModel;
            _container = container;
            _deviceConfiguration = deviceConfiguration;
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelGettingFunc =
                container.Resolve<Func<IFragmentOptionGroupViewModel>>();
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelGettingFunc =
                container.Resolve<Func<IFragmentOptionCommandViewModel>>();

            IFragmentOptionsViewModel fragmentOptionsViewModel = container.Resolve<IFragmentOptionsViewModel>();


            fragmentOptionsViewModel.FragmentOptionGroupViewModels =
                new ObservableCollection<IFragmentOptionGroupViewModel>();


            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel =
                fragmentOptionGroupViewModelGettingFunc();




            //группа устройство
            fragmentOptionGroupViewModel.NameKey = ApplicationGlobalNames.UiGroupingStrings.DEVICE_STRING_KEY;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();

            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel =
                fragmentOptionCommandViewModelGettingFunc();


            fragmentOptionCommandViewModel.TitleKey = ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxIn;
            this.ReadConfigurationCommand = new RelayCommand(OnExecuteReadConfiguration, () => !this._isQueryInProgress);
            fragmentOptionCommandViewModel.OptionCommand = this.ReadConfigurationCommand;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconChevronRight;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteTransferFromDeviceToLocal);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxOut;
            WriteConfigurationCommand=  new RelayCommand(OnExecuteWriteLocalValuesToDevice, () => !this._isQueryInProgress);
            fragmentOptionCommandViewModel.OptionCommand = this.WriteConfigurationCommand;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.EDIT_LOCAL_CONFIGURATION_VALUES_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconEdit;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteEditLocalValues);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);
            fragmentOptionGroupViewModel = fragmentOptionGroupViewModelGettingFunc();
            //группа файл
            fragmentOptionGroupViewModel.NameKey = ApplicationGlobalNames.UiGroupingStrings.FILE_STRING_KEY;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.LOAD_FROM_FILE_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscUpload;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteLoadConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.SAVE_CONFUGURATION_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscDownload;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteSaveConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.EXPORT_CONFUGURATION_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconPrintText;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteExportConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);



            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);

            // группа дерево
            fragmentOptionGroupViewModel = fragmentOptionGroupViewModelGettingFunc();
            fragmentOptionGroupViewModel.NameKey = ApplicationGlobalNames.UiGroupingStrings.TREE_STRING_KEY;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels = new List<IFragmentOptionCommandViewModel>();

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.EXPAND_LEVEL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconStepInto;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteExpandLevel);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);


            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.COLLAPSE_LEVEL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconStepOut;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteCollapseLevel);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);


            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);

            return fragmentOptionsViewModel;
        }



        private async void OnExecuteReadConfiguration()
        {
            try
            {
                SetQueriesLock(true);
                ReadConfigurationCommand.RaiseCanExecuteChanged();
                await new MemoryReaderVisitor(_deviceConfiguration,
                    _runtimeConfigurationViewModel.DeviceContext, 0).ExecuteRead();
            }
            finally
            {
                SetQueriesLock(false);
                ReadConfigurationCommand.RaiseCanExecuteChanged();
            }

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

                if (ExpandLevelByIndex(configurationItemViewModel.ChildStructItemViewModels.ToList(),
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
                if (CollapseLevelByIndex(configurationItemViewModel.ChildStructItemViewModels.ToList(),
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
            if (ExpandLevelByIndex(
                _runtimeConfigurationViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>()
                    .ToList(), _levelIndex,
                0))
                _levelIndex++;
        }

        private void OnExecuteCollapseLevel()
        {
            if (CollapseLevelByIndex(
                _runtimeConfigurationViewModel.RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>()
                    .ToList(),
                _levelIndex, 0))
                _levelIndex--;

        }

        private void OnExecuteLoadConfiguration()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                var loadedLoaclMemory = _container.Resolve<ISerializerService>()
                      .DeserializeFromFile<Dictionary<ushort, ushort>>(ofd.FileName);
                _runtimeConfigurationViewModel.DeviceContext.DeviceMemory.LocalMemoryValues = loadedLoaclMemory;
                var addresses = loadedLoaclMemory.Keys.ToArray();
                foreach (var address in addresses)
                {
	                _runtimeConfigurationViewModel.DeviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
		                address, 1, MemoryKind.UshortMemory);
                }
                
            }
        }

        private void OnExecuteExportConfiguration()
        {
            ConfigurationExportHelper.ExportConfiguration(
                 _runtimeConfigurationViewModel, _container,
                 _runtimeConfigurationViewModel.DeviceContext.DeviceName, _runtimeConfigurationViewModel.NameForUiKey);
        }

        private void OnExecuteSaveConfiguration()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".cnf"; 
            var localizer = _container.Resolve<ILocalizerService>();
            var nameForUiLocalized = _runtimeConfigurationViewModel.NameForUiKey;
            localizer.TryGetLocalizedString(_runtimeConfigurationViewModel.NameForUiKey, out nameForUiLocalized);
            sfd.FileName = nameForUiLocalized+" "+ _runtimeConfigurationViewModel.DeviceContext.DeviceName;
            if (sfd.ShowDialog() == true)
            {
                _container.Resolve<ISerializerService>().SerializeInFile(
                    _runtimeConfigurationViewModel.DeviceContext.DeviceMemory.LocalMemoryValues,
                    sfd.FileName);
            }
        }

        private async void OnExecuteWriteLocalValuesToDevice()
        {

            try
            {
                SetQueriesLock(true);
                this.WriteConfigurationCommand.RaiseCanExecuteChanged();
                if (LocalValuesWriteValidator.ValidateLocalValuesToWrite(_runtimeConfigurationViewModel
                    .RootConfigurationItemViewModels))
                {
                    await new MemoryWriterVisitor(_runtimeConfigurationViewModel.DeviceContext, new List<ushort>(),
                        _deviceConfiguration, 0).ExecuteWrite();
                }
            }
            finally
            {
                SetQueriesLock(false);
                WriteConfigurationCommand.RaiseCanExecuteChanged();
            }

         
        }

        private void OnExecuteEditLocalValues()
        {
            //
        }

        private async void OnExecuteTransferFromDeviceToLocal()
        {
            var memoryAccessor = new ConfigurationMemoryAccessor(_deviceConfiguration,
                _runtimeConfigurationViewModel.DeviceContext, MemoryAccessEnum.TransferFromDeviceToLocal);
            await memoryAccessor.Process();
        }
    }
}