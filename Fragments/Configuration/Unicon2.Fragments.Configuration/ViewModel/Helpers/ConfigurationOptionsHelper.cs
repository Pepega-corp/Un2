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
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteReadConfiguration);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconChevronRight;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteTransferFromDeviceToLocal);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelGettingFunc();
            fragmentOptionCommandViewModel.TitleKey = ConfigurationKeys.WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxOut;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteWriteLocalValuesToDevice);
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
            await new MemoryReaderVisitor(_deviceConfiguration,
                _runtimeConfigurationViewModel.DeviceEventsDispatcher,
                _runtimeConfigurationViewModel.DeviceMemory).ExecuteRead();
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
                //  IDeviceMemory loadedConfigMemory = _container.Resolve<ISerializerService>()
                //      .DeserializeFromFile<IDeviceMemory>(ofd.FileName);
                // (_runtimeConfigurationViewModel.Model as IDeviceConfiguration).DeviceMemory =
                //      loadedConfigMemory;
            }
        }

        private void OnExecuteExportConfiguration()
        {
            // ConfigurationExportHelper.ExportConfiguration(
            //      _runtimeConfigurationViewModel.Model as IDeviceConfiguration, _container,
            //      _runtimeConfigurationViewModel.GetDeviceName(), _runtimeConfigurationViewModel.NameForUiKey);
        }

        private void OnExecuteSaveConfiguration()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = " CNF файл (*.cnf)|*.cnf" + "|Все файлы (*.*)|*.* ";
            sfd.DefaultExt = ".cnf";
            sfd.FileName = _runtimeConfigurationViewModel.NameForUiKey;
            if (sfd.ShowDialog() == true)
            {
                //   _container.Resolve<ISerializerService>().SerializeInFile(
                //       (_runtimeConfigurationViewModel.Model as IDeviceConfiguration).DeviceMemory,
                //       sfd.FileName);
            }
        }

        private async void OnExecuteWriteLocalValuesToDevice()
        {
            await new MemoryWriterVisitor(_deviceConfiguration,
                _runtimeConfigurationViewModel.DeviceEventsDispatcher,
                _runtimeConfigurationViewModel.DeviceMemory, new List<ushort>()).ExecuteWrite();
        }

        private async void OnExecuteEditLocalValues()
        {
            //
        }

        private async void OnExecuteTransferFromDeviceToLocal()
        {
            var memoryAccessor = new ConfigurationMemoryAccessor(_deviceConfiguration,
                _runtimeConfigurationViewModel.DeviceEventsDispatcher, MemoryAccessEnum.TransferFromDeviceToLocal,
                _runtimeConfigurationViewModel.DeviceMemory);
            await memoryAccessor.Process();
        }
    }
}