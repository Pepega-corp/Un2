using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ControlzEx.Standard;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Unicon.Common.Model;
using Unicon.Common.Queries;
using Unicon.Common.Result;
using Unicon2.Infrastructure.Common;
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

		public WebSynchronizationViewModel(IApplicationSettingsService applicationSettingsService,
			IDevicesContainerService devicesContainerService)
		{
			_applicationSettingsService = applicationSettingsService;
			_devicesContainerService = devicesContainerService;
			Definitions = new ObservableCollection<DefinitionInfoViewModel>();
			UseDefaultUrl = true;
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
		private bool _useDefaultUrl;

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
			set
			{
				_applicationSettingsService.UrlToServer = value;
				RaisePropertyChanged();
			}
		}

		public string ClientSecret
		{
			get => _applicationSettingsService.ClientSecret;
			set
			{
				_applicationSettingsService.ClientSecret = value;
				RaisePropertyChanged();
			}
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

		public bool UseDefaultUrl
		{
			get => _useDefaultUrl;
			set
			{
				_useDefaultUrl = value; 
				RaisePropertyChanged();
			}
		}

		public async void RefreshAll()
		{
			try
			{
				using (var httpClient = new HttpClient())
				{
					await LoadLocalDefinitions();
					if (await LoadServerUrl())
					{
						var test = await (await httpClient.GetAsync(UrlToServer)).Content.ReadAsStringAsync();
						if (!string.IsNullOrWhiteSpace(test))
						{
							CanConnectToServer = true;
							await LoadRemoteDefinitions();
							return;
						}

						CanConnectToServer = false;
					}
				}
			}
			catch(Exception e)
			{
				CanConnectToServer = false;
			}
		}

		private async Task LoadRemoteDefinitions()
		{
			using (var httpClient = new HttpClient())
			{
				var commandString = new GetDeviceDefinitionsQuery();
				var response = await httpClient.PostAsync(UrlToServer + "api/v1/root/command", new StringContent(
					JsonConvert.SerializeObject(
						commandString, new JsonSerializerSettings()
						{
							TypeNameHandling = TypeNameHandling.All,
                        }), Encoding.UTF8, "application/json"));
			    var str = await response.Content.ReadAsStringAsync();
				var result =
					JsonConvert.DeserializeObject<Result<List<DeviceDefinition>>>(str,new JsonSerializerSettings()
					{
					    TypeNameHandling = TypeNameHandling.All
                    });
			}
		}

		private async Task<bool> LoadServerUrl()
		{
			if (!UseDefaultUrl) return true;
			using (var httpClient=new HttpClient())
			{
				var response=await httpClient.GetStringAsync(
					"https://docs.google.com/document/d/1TMzTTGtSWLjgFhiOPl8Q9usSt0PF0HBCpz-zHsHc_wI");
				var x=Regex.Matches(response, "ReferenceForUnicon: {(\\S*)}");
				try
				{
					var url = x[0].Groups[1].Value;
					UrlToServer = url;
					return true;
				}
				catch (Exception)
				{
					CanConnectToServer = false;
					return false;
				}
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
				new RelayCommand(() =>
				{

				}, () => false),
				new RelayCommand(() =>
				{
					_devicesContainerService.DeleteDeviceDefinition(creator.DeviceName);
					RefreshAll();
				}, () => true));
		}
		//private DefinitionInfoViewModel CreateRemoteDefinitionInfoViewModel(DeviceDefinition deviceDefinition)
		//{
		//	return new DefinitionInfoViewModel(deviceDefinition.Id.ToString(), deviceDefinition..DeviceName,
		//		creator.DeviceMetaInfo.LastEditedDateTime,
		//		new RelayCommand(() => { }, () => false),
		//		new RelayCommand(() => { }, () => false),
		//		new RelayCommand(() =>
		//		{
		//			_devicesContainerService.DeleteDeviceDefinition(creator.DeviceName);
		//			RefreshAll();
		//		}, () => true));
		//}
	}
  
}