using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Web.Presentation.ViewModels
{
	public class WebSynchronizationViewModel : ViewModelBase
	{
		private readonly IApplicationSettingsService _applicationSettingsService;
		private readonly IDevicesContainerService _devicesContainerService;
		private readonly HttpClient _httpClient;

		public WebSynchronizationViewModel(IApplicationSettingsService applicationSettingsService,
			IDevicesContainerService devicesContainerService)
		{
			_applicationSettingsService = applicationSettingsService;
			_devicesContainerService = devicesContainerService;
			_httpClient = new HttpClient();
			Definitions = new ObservableCollection<DefinitionInfoViewModel>();
			//RefreshAll = new RelayCommand();
			ToggleSettingsPopup = new RelayCommand(OnTogglePopup);
			RefreshAll();
		}

		private void OnTogglePopup()
		{
			IsSettingsOpen = !IsSettingsOpen;
		}

		private bool _canConnectToServer;
		private bool _isSettingsOpen;

		public bool CanConnectToServer
		{
			get => _canConnectToServer;
			set
			{
				_canConnectToServer = value;
				RaisePropertyChanged();
			}
		}

		public ICommand RefreshAllCommand { get; }

		public ObservableCollection<DefinitionInfoViewModel> Definitions { get; }

		public string UrlToServer
		{
			get => _applicationSettingsService.UrlToServer;
			set => _applicationSettingsService.UrlToServer = value;
		}

		public string ClientSecret
		{
			get => _applicationSettingsService.ClientSecret;
			set => _applicationSettingsService.ClientSecret = value;
		}

		public bool IsSettingsOpen
		{
			get => _isSettingsOpen;
			set
			{
				_isSettingsOpen = value;
				RaisePropertyChanged();
			}
		}

		public ICommand ToggleSettingsPopup { get; }

		public async void RefreshAll()
		{
			try
			{
				await LoadLocalDefinitions();
				var test = await (await _httpClient.GetAsync(UrlToServer)).Content.ReadAsStringAsync();
				if (!string.IsNullOrWhiteSpace(test))
				{
					CanConnectToServer = true;
					return;
				}

				CanConnectToServer = false;
			}
			catch
			{
				CanConnectToServer = false;
			}
		}

		private async Task LoadLocalDefinitions()
		{
			_devicesContainerService.LoadDevicesDefinitions();
			var definitionInfoViewModels = _devicesContainerService.Creators.Select(
				creator => CreateLocalDefinitionInfoViewModel(creator)
			);
			Definitions.Clear();
			Definitions.AddCollection(definitionInfoViewModels);
		}

		private DefinitionInfoViewModel CreateLocalDefinitionInfoViewModel(IDeviceCreator creator)
		{
			return new DefinitionInfoViewModel(creator.DeviceMetaInfo.Id.ToString(), creator.DeviceName,
				creator.DeviceMetaInfo.LastEditedDateTime,
				new RelayCommand(() => { }, () => false),
				new RelayCommand(() => { }, () => false),
				new RelayCommand(() =>
				{
					_devicesContainerService.DeleteDeviceDefinition(creator.DeviceName);
					RefreshAll();
				}, () => true));
		}
	}
}