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

        public MeasuringMonitorViewModel(IMeasuringGroupViewModelFactory measuringGroupViewModelFactory,
            IFragmentOptionsViewModel fragmentOptionsViewModel,
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelgetFunc,
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelgetFunc,
            Func<IFragmentOptionToggleCommandViewModel> fragmentOptionToggleCommandViewModelgetFunc)
        {
            this._measuringGroupViewModelFactory = measuringGroupViewModelFactory;
            this.MeasuringGroupViewModels = new ObservableCollection<IMeasuringGroupViewModel>();
            this.FragmentOptionsViewModel = fragmentOptionsViewModel;
            this.MeasuringElementViewModels = new ObservableCollection<IMeasuringElementViewModel>();

            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Loading";

            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Load";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(async () =>
            {
                if (this._isQueriesStarted) return;
                if (this.IsListViewSelected)
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



            this.FragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);


            fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Presentation";
            fragmentOptionCommandViewModel = fragmentOptionToggleCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "ViewAll";
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand<bool?>(isAllSelected =>
            {
                if (isAllSelected.HasValue)
                {
                    if (this._isQueriesStarted)
                    {
                        this.IsListViewSelected = isAllSelected.Value;
                    }
                    else
                    {
                        this.IsListViewSelected = isAllSelected.Value;
                    }
                }
            });
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconAlignJustify;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);
            this.FragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);

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

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            this._measuringMonitor = value as IMeasuringMonitor;
            this.MeasuringGroupViewModels.Clear();
            this.MeasuringElementViewModels.Clear();

            foreach (IMeasuringGroup measuringGroup in this._measuringMonitor.MeasuringGroups)
            {
                IMeasuringGroupViewModel group = this._measuringGroupViewModelFactory.CreateMeasuringGroupViewModel(measuringGroup);
                this.MeasuringGroupViewModels.Add(group);
                this.MeasuringElementViewModels.AddCollection(group.MeasuringElementViewModels);
            }
            if (this.MeasuringGroupViewModels.Count > 0)
            {
                this.SelectedMeasuringGroupViewModel = this.MeasuringGroupViewModels[0];
            }
            this.MeasuringElementListCollectionView = new ListCollectionView(this.MeasuringElementViewModels.ToList());
            this.MeasuringElementListCollectionView.GroupDescriptions.Add(
                new PropertyGroupDescription(nameof(IMeasuringElementViewModel.GroupName)));
        //    this._measuringMonitor.SetSelectedGroups(this._measuringMonitor.MeasuringGroups);
        }

        private object GetModel()
        {
            this._measuringMonitor.MeasuringGroups.Clear();
            foreach (IMeasuringGroupViewModel measuringGroupViewModel in this.MeasuringGroupViewModels)
            {
                this._measuringMonitor.MeasuringGroups.Add(measuringGroupViewModel.Model as IMeasuringGroup);
            }
            return this._measuringMonitor;
        }

        public string NameForUiKey => MeasuringKeys.MEASURING_MONITOR;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        public ObservableCollection<IMeasuringGroupViewModel> MeasuringGroupViewModels { get; set; }

        public IMeasuringGroupViewModel SelectedMeasuringGroupViewModel
        {
            get { return this._selectedMeasuringGroupViewModel; }
            set
            {
                if (this._isQueriesStarted)
                {
                    this._selectedMeasuringGroupViewModel = value;
                }
                else
                {
                    this._selectedMeasuringGroupViewModel = value;
                }
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }

        public CollectionView MeasuringElementListCollectionView
        {
            get { return this._measuringElementListCollectionView; }
            set
            {

                this._measuringElementListCollectionView = value;

                this.RaisePropertyChanged();
            }
        }

        public bool IsListViewSelected
        {
            get { return this._isListViewSelected; }
            set
            {
                this._isListViewSelected = value;
                this.RaisePropertyChanged();
            }
        }

        protected override void OnDisposing()
        {
            base.OnDisposing();
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
	        throw new NotImplementedException();
        }

        public void SetDeviceData(string deviceName, IDeviceEventsDispatcher deviceEventsDispatcher, IDeviceMemory deviceMemory)
        {
	        throw new NotImplementedException();
        }

        public string GetDeviceName()
        {
	        throw new NotImplementedException();
        }
    }
}