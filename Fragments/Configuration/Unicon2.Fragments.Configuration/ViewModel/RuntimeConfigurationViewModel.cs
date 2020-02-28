using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
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
        private readonly IRuntimeConfigurationItemViewModelFactory _runtimeConfigurationItemViewModelFactory;


        public RuntimeConfigurationViewModel(ITypesContainer container,
            IRuntimeConfigurationItemViewModelFactory runtimeConfigurationItemViewModelFactory)
        {
            this._container = container;
            this._runtimeConfigurationItemViewModelFactory = runtimeConfigurationItemViewModelFactory;

            this.AllRows = new ObservableCollection<IRuntimeConfigurationItemViewModel>();
            this.MainRows = new ObservableCollection<MainConfigItemViewModel>();

            this.RootConfigurationItemViewModels = new ObservableCollection<IRuntimeConfigurationItemViewModel>();
            MainItemSelectedCommand = new RelayCommand<object>(OnMainItemSelected);
            ShowTableCommand = new RelayCommand<object>(OnShowTable);
        }



        private void OnShowTable(object obj)
        {
            if (!(obj is MainConfigItemViewModel mainItem)) return;
            if (SelectedConfigDetails is MainConfigItemViewModel selectedConfigDetails)
            {
                selectedConfigDetails.IsSelected = false;
                selectedConfigDetails.IsTableSelected = false;
            }

            mainItem.IsTableSelected = true;
            if (mainItem.RelatedConfigurationItemViewModel is IAsTableViewModel tableViewModel)
            {
                tableViewModel.IsTableView = true;
            }

            mainItem.IsSelected = true;
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


        private ObservableCollection<IRuntimeConfigurationItemViewModel> _allRows;
        private IFragmentOptionsViewModel _fragmentOptionsViewModel;
        private ObservableCollection<IRuntimeConfigurationItemViewModel> _rootConfigurationItemViewModels;
        private string _deviceName;
        private ObservableCollection<MainConfigItemViewModel> _mainRows;
        private object _selectedConfigDetails;
        private string _nameForUiKey;

        public ICommand MainItemSelectedCommand { get; }

        public ObservableCollection<IRuntimeConfigurationItemViewModel> RootConfigurationItemViewModels
        {
            get { return this._rootConfigurationItemViewModels; }
            set
            {
                this._rootConfigurationItemViewModels = value;
                this.RaisePropertyChanged();

            }
        }

        public ObservableCollection<MainConfigItemViewModel> MainRows
        {
            get { return this._mainRows; }
            set
            {
                this._mainRows = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IRuntimeConfigurationItemViewModel> AllRows
        {
            get { return this._allRows; }
            set
            {
                this._allRows = value;
                this.RaisePropertyChanged();
            }
        }

        public IDeviceEventsDispatcher DeviceEventsDispatcher { get; set; }

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.RUNTIME_CONFIGURATION_VIEWMODEL;

        public string NameForUiKey => _nameForUiKey;


        public IFragmentOptionsViewModel FragmentOptionsViewModel
        {
            get { return this._fragmentOptionsViewModel; }
            set
            {
                this._fragmentOptionsViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            this.AllRows.Clear();
            this.RootConfigurationItemViewModels.Clear();
            if (!(deviceFragment is IDeviceConfiguration deviceConfiguration)) return;

            _nameForUiKey = deviceConfiguration.StrongName;
            if (deviceConfiguration.RootConfigurationItemList != null)
            {
                foreach (IConfigurationItem configurationItem in deviceConfiguration.RootConfigurationItemList)
                {
                    this.RootConfigurationItemViewModels.Add(
                        configurationItem.Accept(_runtimeConfigurationItemViewModelFactory));
                }
            }
            
            this.AllRows.AddCollection(this.RootConfigurationItemViewModels);
            this.FragmentOptionsViewModel =
                (new ConfigurationOptionsHelper()).CreateConfigurationFragmentOptionsViewModel(this, _container,
                    deviceConfiguration);
        }

       


        //private ObservableCollection<MainConfigItemViewModel> FilterMainConfigItems(
        //    IEnumerable<IConfigurationItemViewModel> rootItems, bool isParentReiterable,
        //    IGroupWithReiterationInfo groupWithReiterationInfo = null)
        //{
        //    var resultCollection = new ObservableCollection<MainConfigItemViewModel>();
        //    resultCollection.AddCollection(rootItems.Where(item =>
        //        item is IItemGroupViewModel itemGroupViewModel &&
        //        ((itemGroupViewModel.Model as IItemsGroup).IsMain ?? true)).Select(item =>
        //    {
        //        //if (item is IItemGroupViewModel groupViewModelWithreit &&
        //        //    groupViewModelWithreit.Model is IItemsGroup itemsGroup &&
        //        //    itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiteration &&
        //        //    groupWithReiteration.IsReiterationEnabled)
        //        //{
        //        //    return new MainConfigItemViewModel(
        //        //        FilterMainConfigItems(item.ChildStructItemViewModels, true,groupWithReiterationInfo.SubGroups), item);
        //        //}
        //        //else
        //        //{
        //        return new MainConfigItemViewModel(
        //            FilterMainConfigItems(item.ChildStructItemViewModels, isParentReiterable), item);


        //    }));
        //    return resultCollection;
        //}

     

        public void SetDeviceData(string deviceName, IDeviceEventsDispatcher deviceEventsDispatcher)
        {
            DeviceEventsDispatcher = deviceEventsDispatcher;
            _runtimeConfigurationItemViewModelFactory.Initialize(deviceEventsDispatcher);
            _deviceName = deviceName;
        }

        public string GetDeviceName()
        {
            return _deviceName;
        }
    }
}