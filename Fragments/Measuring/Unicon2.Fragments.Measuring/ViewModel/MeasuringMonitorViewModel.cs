using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Unicon2.Fragments.Measuring.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.MemoryAccess;
using Unicon2.Fragments.Measuring.Subscriptions;
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
        

		private CollectionView _measuringElementListCollectionView;
		private IMeasuringGroupViewModel _selectedMeasuringGroupViewModel;
		private double _scale;
		private MeasuringLoader _loader;
	    private RelayCommand _readCommand;

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
	        this._readCommand = new RelayCommand(() =>
	        {
	            _loader.ExecuteLoad();
	        }, () => !this._loader.IsLoadInProgress);

	        fragmentOptionCommandViewModel.OptionCommand = this._readCommand;
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
	                    _loader.StartLoading();
	                }
	                else
	                {
	                    _loader.StopLoading();
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
	                IsListViewSelected = isAllSelected.Value;
	                _loader.SetCurrentGroup();
	            }
	        });
	        fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconAlignJustify;
	        fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

	        fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
	        fragmentOptionCommandViewModel.TitleKey = "ZoomIn";
	        fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(() => { Scale += 0.1; });
	        fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconMagnifyAdd;

	        fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

	        fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
	        fragmentOptionCommandViewModel.TitleKey = "ZoomOut";
	        fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(() => { Scale -= 0.1; });
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
				_selectedMeasuringGroupViewModel = value;
				_loader?.SetCurrentGroup(_selectedMeasuringGroupViewModel.Header);
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
			MeasuringSubscriptionSet measuringSubscriptionSet = new MeasuringSubscriptionSet();
			foreach (IMeasuringGroup measuringGroup in _measuringMonitor.MeasuringGroups)
			{
				IMeasuringGroupViewModel group =
					_measuringGroupViewModelFactory.CreateMeasuringGroupViewModel(measuringGroup,
						measuringSubscriptionSet,DeviceContext);
				MeasuringGroupViewModels.Add(group);
				MeasuringElementViewModels.AddCollection(group.MeasuringElementViewModels);
			}
			MeasuringElementListCollectionView = new ListCollectionView(MeasuringElementViewModels.ToList());
			MeasuringElementListCollectionView.GroupDescriptions.Add(
				new PropertyGroupDescription(nameof(IMeasuringElementViewModel.GroupName)));
			_loader = new MeasuringLoader(DeviceContext, measuringSubscriptionSet,_measuringMonitor,this._readCommand);
			if (MeasuringGroupViewModels.Count > 0)
			{
				SelectedMeasuringGroupViewModel = MeasuringGroupViewModels[0];
			}

			//    this._measuringMonitor.SetSelectedGroups(this._measuringMonitor.MeasuringGroups);
		}

		public DeviceContext DeviceContext { get; set; }
	}
}