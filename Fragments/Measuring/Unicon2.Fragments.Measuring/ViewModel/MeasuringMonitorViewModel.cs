using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.Icons;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.ViewModel
{
    public class MeasuringMonitorViewModel : DisposableBindableBase, IMeasuringMonitorViewModel
    {
        private readonly IMeasuringGroupViewModelFactory _measuringGroupViewModelFactory;

        private IMeasuringMonitor _measuringMonitor;
        private bool _isListViewSelected;
        private bool _isQueriesStarted;

        private CollectionView _measuringElementListCollectionView;
        private IMeasuringGroupViewModel _selectedMeasuringGroupViewModel;
        private double _scale;

        public MeasuringMonitorViewModel(IMeasuringGroupViewModelFactory measuringGroupViewModelFactory,
            IFragmentOptionsViewModel fragmentOptionsViewModel,
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelgetFunc,
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelgetFunc,
            Func<IFragmentOptionToggleCommandViewModel> fragmentOptionToggleCommandViewModelgetFunc)
        {
            _measuringGroupViewModelFactory = measuringGroupViewModelFactory;
            MeasuringGroupViewModels = new ObservableCollection<IMeasuringGroupViewModel>();
            FragmentOptionsViewModel = fragmentOptionsViewModel;
            MeasuringElementViewModels = new ObservableCollection<IMeasuringElementViewModel>();

            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Loading";

            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Load";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(async () =>
            {
                if (_isQueriesStarted) return;
                if (IsListViewSelected)
                {
					//todo
                    //this._measuringMonitor.SetSelectedGroups(this._measuringMonitor.MeasuringGroups);
                }
                else
                {
                    //if (this._selectedMeasuringGroupViewModel == null) return;
                    //this._measuringMonitor.SetSelectedGroups(
                    //    new List<IMeasuringGroup>() { this._selectedMeasuringGroupViewModel.Model as IMeasuringGroup });
                }
            });
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxIn;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionToggleCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "CycleLoading";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand<bool?>(isCycleLoadingEnabled =>
            {
                if (isCycleLoadingEnabled.HasValue)
                {
                    if (isCycleLoadingEnabled.Value)
                    {
                    }
                    else
                    {
                    }
                }
            });
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconArrowRightLeft;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);



            FragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);


            fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Presentation";
            fragmentOptionCommandViewModel = fragmentOptionToggleCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "ViewAll";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand<bool?>(isAllSelected =>
            {
                if (isAllSelected.HasValue)
                {
                    if (_isQueriesStarted)
                    {
                        IsListViewSelected = isAllSelected.Value;
                    }
                    else
                    {
                        IsListViewSelected = isAllSelected.Value;
                    }
                }
            });
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconAlignJustify;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);






            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "ZoomIn";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(async () => { Scale += 0.1; });
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconMagnifyAdd;

            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "ZoomOut";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(async () => { Scale -= 0.1; });
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconMagnifyMinus;

            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            FragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);
            Scale = 1;
        }

        public double Scale
        {
	        get => _scale;
	        set
	        {
		        _scale = value; 
                RaisePropertyChanged();
	        }
        }


        //private void StartCycleLoading()
        //{
        //    this._isQueriesStarted = true;
        //    if (this.IsListViewSelected)
        //    {
        //        this._measuringMonitor.SetSelectedGroups(this._measuringMonitor.MeasuringGroups);
        //    }
        //    else
        //    {
        //        this._measuringMonitor.SetSelectedGroups(
        //            new List<IMeasuringGroup>() { this._selectedMeasuringGroupViewModel.Model as IMeasuringGroup });
        //    }
        //}


        public string StrongName => MeasuringKeys.MEASURING_MONITOR +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public string NameForUiKey => MeasuringKeys.MEASURING_MONITOR;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        public ObservableCollection<IMeasuringGroupViewModel> MeasuringGroupViewModels { get; set; }

        public IMeasuringGroupViewModel SelectedMeasuringGroupViewModel
        {
            get { return _selectedMeasuringGroupViewModel; }
            set
            {
                if (_isQueriesStarted)
                {
                    _selectedMeasuringGroupViewModel = value;
                }
                else
                {
                    _selectedMeasuringGroupViewModel = value;
                }
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }

        public CollectionView MeasuringElementListCollectionView
        {
            get { return _measuringElementListCollectionView; }
            set
            {

                _measuringElementListCollectionView = value;

                RaisePropertyChanged();
            }
        }

        public bool IsListViewSelected
        {
            get { return _isListViewSelected; }
            set
            {
                _isListViewSelected = value;
                RaisePropertyChanged();
            }
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            _measuringMonitor = deviceFragment as IMeasuringMonitor;
            MeasuringGroupViewModels.Clear();
            MeasuringElementViewModels.Clear();

            foreach (IMeasuringGroup measuringGroup in _measuringMonitor.MeasuringGroups)
            {
                IMeasuringGroupViewModel group = _measuringGroupViewModelFactory.CreateMeasuringGroupViewModel(measuringGroup);
                MeasuringGroupViewModels.Add(group);
                MeasuringElementViewModels.AddCollection(group.MeasuringElementViewModels);
            }
            if (MeasuringGroupViewModels.Count > 0)
            {
                SelectedMeasuringGroupViewModel = MeasuringGroupViewModels[0];
            }
            MeasuringElementListCollectionView = new ListCollectionView(MeasuringElementViewModels.ToList());
            MeasuringElementListCollectionView.GroupDescriptions.Add(
                new PropertyGroupDescription(nameof(IMeasuringElementViewModel.GroupName)));
            //    this._measuringMonitor.SetSelectedGroups(this._measuringMonitor.MeasuringGroups);
        }

        public DeviceContext DeviceContext { get; set; }
    }
}