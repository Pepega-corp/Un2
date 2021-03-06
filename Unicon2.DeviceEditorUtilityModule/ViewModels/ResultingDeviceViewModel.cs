﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResultingDeviceViewModel : ViewModelBase, IResultingDeviceViewModel
    {
        private IDevice _device;
        private ITypesContainer _container;
        private IDeviceSharedResources _deviceSharedResources;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private readonly IFragmentEditorViewModelFactory _fragmentEditorViewModelFactory;
        private readonly IConnectionStateViewModelFactory _connectionStateViewModelFactory;
        private readonly ISerializerService _serializerService;
        private string _deviceName;
        private ObservableCollection<IFragmentEditorViewModel> _fragmentEditorViewModels;
        private IFragmentEditorViewModel _selectedFragmentEditorViewModel;

        public ResultingDeviceViewModel(IDevice device, ITypesContainer container, ILocalizerService localizerService,
            IDeviceSharedResources deviceSharedResources, IApplicationGlobalCommands applicationGlobalCommands,
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,
            IFragmentEditorViewModelFactory fragmentEditorViewModelFactory,
            IConnectionStateViewModelFactory connectionStateViewModelFactory, ISerializerService serializerService)
        {
            _device = device;
            _container = container;
            _deviceSharedResources = deviceSharedResources;
            _applicationGlobalCommands = applicationGlobalCommands;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            _fragmentEditorViewModelFactory = fragmentEditorViewModelFactory;
            _connectionStateViewModelFactory = connectionStateViewModelFactory;
            _serializerService = serializerService;

            DeviceName =
                localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.NEW_DEVICE_STRING);
            FragmentEditorViewModels = new ObservableCollection<IFragmentEditorViewModel>();
            sharedResourcesGlobalViewModel.InitializeFromResources(deviceSharedResources);
            NavigateToConnectionTestingCommand = new RelayCommand(OnNavigateToConnectionTestingExecute);
        }

        private void OnNavigateToConnectionTestingExecute()
        {
            _applicationGlobalCommands.ShowWindowModal(
                () => new ConnectionTestingWindow(),
                _connectionStateViewModelFactory.CreateConnectionStateViewModel(_device.ConnectionState));
        }



        public IFragmentEditorViewModel SelectedFragmentEditorViewModel
        {
	        get => _selectedFragmentEditorViewModel;
	        set
	        {
		        _selectedFragmentEditorViewModel = value;
		        RaisePropertyChanged();
	        }
        }
        public ICommand NavigateToConnectionTestingCommand { get; }


        public void OpenSharedResources()
        {
            _sharedResourcesGlobalViewModel.OpenSharedResourcesForEditing();
        }



        public ObservableCollection<IFragmentEditorViewModel> FragmentEditorViewModels
        {
            get { return _fragmentEditorViewModels; }
            set
            {
                _fragmentEditorViewModels = value;
                RaisePropertyChanged();
            }
        }


        public void LoadDevice(string path)
        {
            LoadDevice(_serializerService.DeserializeFromFile<IDevice>(path));
        }

        public void LoadDevice(IDevice device)
        {
            _device = device;
            FragmentEditorViewModels.Clear();
            _sharedResourcesGlobalViewModel.InitializeFromResources(_device.DeviceSharedResources);
            foreach (IDeviceFragment fragment in _device.DeviceFragments)
            {
                IFragmentEditorViewModel fragmentEditorViewModel =
                    _fragmentEditorViewModelFactory.CreateFragmentEditorViewModel(fragment);
                FragmentEditorViewModels.Add(fragmentEditorViewModel);
            }

            SelectedFragmentEditorViewModel = FragmentEditorViewModels.FirstOrDefault();
            _deviceSharedResources = _device.DeviceSharedResources;
            DeviceName = _device.Name;
        }

        public void SaveDevice(string path, bool isDefaultSaving = true)
        {
     
            if (isDefaultSaving)
            {

                if (!(Directory.Exists(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH)))
                {
                    Directory.CreateDirectory(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH);
                }

                path = Path.Combine(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH, path + ".json");
            }

            try
            {

                _serializerService.SerializeInFile(GetDevice(), path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IDevice GetDevice()
        {
            _device.Name = DeviceName;
            _device.DeviceFragments = new List<IDeviceFragment>();
            foreach (IFragmentEditorViewModel fragmentEditorViewModel in FragmentEditorViewModels)
            {
                (_device.DeviceFragments as List<IDeviceFragment>).Add(fragmentEditorViewModel.BuildDeviceFragment());
            }

            _device.DeviceSharedResources = _sharedResourcesGlobalViewModel.GetSharedResources();
            return _device;
        }

  

        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged();
            }
        }

    }
}