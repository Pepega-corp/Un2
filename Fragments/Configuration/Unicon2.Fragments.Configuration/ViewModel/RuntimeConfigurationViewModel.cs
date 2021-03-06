﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
	public class   RuntimeConfigurationViewModel : ViewModelBase, IRuntimeConfigurationViewModel,
		IFragmentConnectionChangedListener, IFragmentOpenedListener, IFragmentFileExtension
	{
		private readonly ITypesContainer _container;
		private readonly IApplicationSettingsService _applicationSettingsService;
		private readonly BaseValuesViewModelFactory _baseValuesViewModelFactory;

		private ObservableCollection<IRuntimeConfigurationItemViewModel> _allRows;
		private IFragmentOptionsViewModel _fragmentOptionsViewModel;
		private ObservableCollection<IRuntimeConfigurationItemViewModel> _rootConfigurationItemViewModels;
		private ObservableCollection<MainConfigItemViewModel> _mainRows;
		private object _selectedConfigDetails;
		private string _nameForUiKey;
		private IDeviceConfiguration _deviceConfiguration;
		private ConfigurationOptionsHelper _configurationOptionsHelper;

		public RuntimeConfigurationViewModel(ITypesContainer container,
			IApplicationSettingsService applicationSettingsService,
			BaseValuesViewModelFactory baseValuesViewModelFactory)
		{
			_container = container;
			_applicationSettingsService = applicationSettingsService;
			_baseValuesViewModelFactory = baseValuesViewModelFactory;

			AllRows = new ObservableCollection<IRuntimeConfigurationItemViewModel>();
			MainRows = new ObservableCollection<MainConfigItemViewModel>();

			RootConfigurationItemViewModels = new ObservableCollection<IRuntimeConfigurationItemViewModel>();
			MainItemSelectedCommand = new RelayCommand<object>(OnMainItemSelected);
			ShowTableCommand = new RelayCommand<object>(OnShowTable);
		}



		private void OnShowTable(object obj)
		{
			if (!(obj is MainConfigItemViewModel mainItem)) return;
			if (SelectedConfigDetails is MainConfigItemViewModel selectedConfigDetails && obj !=
				SelectedConfigDetails)
			{
				selectedConfigDetails.IsSelected = false;
				selectedConfigDetails.IsTableSelected = false;
			}

			mainItem.IsTableSelected = !mainItem.IsTableSelected;
			if (mainItem.RelatedConfigurationItemViewModel is IAsTableViewModel tableViewModel)
			{
				tableViewModel.IsTableView = mainItem.IsTableSelected;
			}

			mainItem.IsSelected = true;
			SelectedConfigDetails = null;
			SelectedConfigDetails = mainItem;

		}

		public RelayCommand<object> ShowTableCommand { get; }


		public object SelectedConfigDetails
		{
			get => _selectedConfigDetails;
			set
			{
				_selectedConfigDetails = value;
				RaisePropertyChanged();
			}
		}

		private void OnMainItemSelected(object obj)
		{
			if (!(obj is MainConfigItemViewModel mainItem)) return;
			if (mainItem.ChildConfigItemViewModels.Any() && !mainItem.IsGroupWithReiteration) return;
			if (mainItem.RelatedConfigurationItemViewModel is IAsTableViewModel tableViewModel)
			{
				tableViewModel.IsTableView = false;
			}

			if (SelectedConfigDetails is MainConfigItemViewModel selectedConfigDetails)
			{
				selectedConfigDetails.IsSelected = false;
				selectedConfigDetails.IsTableSelected = false;
			}

			mainItem.IsSelected = true;
			SelectedConfigDetails = mainItem;
		}


		public ICommand MainItemSelectedCommand { get; }

		public ObservableCollection<IRuntimeConfigurationItemViewModel> RootConfigurationItemViewModels
		{
			get { return _rootConfigurationItemViewModels; }
			set
			{
				_rootConfigurationItemViewModels = value;
				RaisePropertyChanged();

			}
		}

		public ObservableCollection<MainConfigItemViewModel> MainRows
		{
			get { return _mainRows; }
			set
			{
				_mainRows = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<IRuntimeConfigurationItemViewModel> AllRows
		{
			get { return _allRows; }
			set
			{
				_allRows = value;
				RaisePropertyChanged();
			}
		}

		public IRuntimeBaseValuesViewModel BaseValuesViewModel { get; set; }

		public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.RUNTIME_CONFIGURATION_VIEWMODEL;

		public string NameForUiKey => _nameForUiKey;

		public IFragmentOptionsViewModel FragmentOptionsViewModel
		{
			get { return _fragmentOptionsViewModel; }
			set
			{
				_fragmentOptionsViewModel = value;
				RaisePropertyChanged();
			}
		}

		private void ClearDeviceValues(List<IConfigurationItemViewModel> list)
		{
			foreach (var viewModel in list)
			{
				if (viewModel.ChildStructItemViewModels.Count > 0)
				{
					ClearDeviceValues(viewModel.ChildStructItemViewModels.ToList());
				}

				if (viewModel is ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel)
				{
					localAndDeviceValueContainingViewModel.DeviceValue = null;
				}
			}
		}
		
		private async Task ClearValues()
		{
			
			DeviceContext.DeviceMemory.DeviceMemoryValues.Clear();
			ClearDeviceValues(RootConfigurationItemViewModels.Cast<IConfigurationItemViewModel>().ToList());
			
			if (!DeviceContext.DataProviderContainer.DataProvider.IsSuccess)
			{
				await new ConfigurationMemoryAccessor(_deviceConfiguration, DeviceContext,
					MemoryAccessEnum.InitalizeZeroForLocals, false).Process();
			}
		}

        private Result ThrowIfComplexProp(List<IConfigurationItem> configurationList)
        {
            foreach (var configurationItem in configurationList)
            {
                if (configurationItem is IComplexProperty)
                {
					return Result.Create(new Exception("В конфигурации есть как минимум одно составное свойство. Пожалуйста сделайте миграцию в редакторе"));
                }
                if (configurationItem is IItemsGroup group)
                {
                    var res=ThrowIfComplexProp(group.ConfigurationItemList);
                    if (!res.IsSuccess)
                    {
                        return res;
                    }
                }
            }
			return Result.Create(true);
        }
		
		public Result Initialize(IDeviceFragment deviceFragment)
		{

            AllRows.Clear();
			RootConfigurationItemViewModels.Clear();


			if (!(deviceFragment is IDeviceConfiguration deviceConfiguration)) return Result.Create(false);
            var valres = ThrowIfComplexProp(deviceConfiguration.RootConfigurationItemList);
            if (!valres.IsSuccess)
            {
                return valres;
            }
            _deviceConfiguration = deviceConfiguration;

			_nameForUiKey = deviceConfiguration.StrongName;
			if (deviceConfiguration.RootConfigurationItemList != null)
			{
				foreach (IConfigurationItem configurationItem in deviceConfiguration.RootConfigurationItemList)
				{

					configurationItem.Accept(
							new RuntimeConfigurationItemViewModelFactory(_container, DeviceContext))
						.OnAddingNeeded(RootConfigurationItemViewModels.Add);
				}
			}
			BaseValuesViewModel = _baseValuesViewModelFactory.CreateRuntimeBaseValuesViewModel(deviceConfiguration.BaseValues,DeviceContext);
			AllRows.AddCollection(RootConfigurationItemViewModels);
			_configurationOptionsHelper = new ConfigurationOptionsHelper();
			FragmentOptionsViewModel =
				_configurationOptionsHelper.CreateConfigurationFragmentOptionsViewModel(this, _container,
					deviceConfiguration);
			MainRows = FilterMainConfigItems(RootConfigurationItemViewModels);
			ClearValues();
            return Result.Create(true);
        }

		private ObservableCollection<MainConfigItemViewModel> FilterMainConfigItems(
			IEnumerable<IConfigurationItemViewModel> rootItems)
		{
			var resultCollection = new ObservableCollection<MainConfigItemViewModel>();
			resultCollection.AddCollection(rootItems.Where(item =>
				item is IItemGroupViewModel itemGroupViewModel &&
				itemGroupViewModel.IsMain).Select(item => new MainConfigItemViewModel(
				FilterMainConfigItems(item.ChildStructItemViewModels), item)));
			return resultCollection;
		}

		public DeviceContext DeviceContext { get; set; }

		private bool _needRefreshValues = false;
		private bool _isOpened;

		public async Task OnConnectionChanged()
		{
			await ClearValues();
			_needRefreshValues = true;
			await RefreshValuesIfNeeded();
			FragmentOptionsViewModel.FragmentOptionGroupViewModels.ForEach(model =>
				model.FragmentOptionCommandViewModels.ForEach(viewModel =>
					(viewModel.OptionCommand as RelayCommand)?.RaiseCanExecuteChanged()));
		}

		private async Task RefreshValuesIfNeeded()
		{
			if (_needRefreshValues && _isOpened && _applicationSettingsService.IsFragmentAutoLoadEnabled)
			{
				if (DeviceContext.DataProviderContainer.DataProvider.IsSuccess)
				{
					await _configurationOptionsHelper.ReadConfiguration(true);
				}

				var localAddresses = DeviceContext.DeviceMemory.LocalMemoryValues.Select(pair => pair.Key).ToList();
				var devAddresses = DeviceContext.DeviceMemory.DeviceMemoryValues.Select(pair => pair.Key).ToList();

				localAddresses.ForEach(address =>
					DeviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(address, 1));
				devAddresses.ForEach(address =>
					DeviceContext.DeviceEventsDispatcher.TriggerDeviceAddressSubscription(address, 1));
				_needRefreshValues = false;

            }

		}

		public async Task Read()
		{
			await _configurationOptionsHelper.ReadConfiguration(true);
		}

		public async Task SetFragmentOpened(bool isOpened)
		{
			_isOpened = isOpened;
			await RefreshValuesIfNeeded();
		}

		public string FileExtension => "cnf";
	}
}