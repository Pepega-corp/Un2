using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
	public class RuntimeConfigurationViewModel : ViewModelBase, IRuntimeConfigurationViewModel
	{
		private readonly ITypesContainer _container;

		private ObservableCollection<IRuntimeConfigurationItemViewModel> _allRows;
		private IFragmentOptionsViewModel _fragmentOptionsViewModel;
		private ObservableCollection<IRuntimeConfigurationItemViewModel> _rootConfigurationItemViewModels;
		private ObservableCollection<MainConfigItemViewModel> _mainRows;
		private object _selectedConfigDetails;
		private string _nameForUiKey;

		public RuntimeConfigurationViewModel(ITypesContainer container)
		{
			_container = container;

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

		public void Initialize(IDeviceFragment deviceFragment)
		{
			AllRows.Clear();
			RootConfigurationItemViewModels.Clear();
			if (!(deviceFragment is IDeviceConfiguration deviceConfiguration)) return;

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

			AllRows.AddCollection(RootConfigurationItemViewModels);
			FragmentOptionsViewModel =
				(new ConfigurationOptionsHelper()).CreateConfigurationFragmentOptionsViewModel(this, _container,
					deviceConfiguration);
			MainRows = FilterMainConfigItems(RootConfigurationItemViewModels);
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
	}
}