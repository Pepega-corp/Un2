using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Helpers;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel
{
	public class PresentationSettingsViewModel : ViewModelBase
	{
		private readonly IMeasuringGroupEditorViewModel _measuringGroupEditorViewModel;
		private PresentationElementViewModel _selectedElementViewModel;
		private readonly string _allString = "All";
		private readonly string _groupsString = "Groups";

		private string _selectedFilterString;
		private List<PresentationElementViewModel> _filteredPresentationElementViewModels;
		private IMeasuringElementViewModelFactory _measuringElementViewModelFactory;
		private Dictionary<Guid, PositioningInfoViewModel> _positioningInfosDictionary;
		public PresentationSettingsViewModel(IMeasuringGroupEditorViewModel measuringGroupEditorViewModel, Dictionary<Guid, PositioningInfoViewModel> positioningInfosDictionary)
		{
			_measuringGroupEditorViewModel = measuringGroupEditorViewModel;
			_positioningInfosDictionary = positioningInfosDictionary;
			PresentationElementViewModels = new ObservableCollection<PresentationElementViewModel>();
			AddGroupCommand = new RelayCommand(OnAddGroup);
			DeleteGroupCommand = new RelayCommand(OnDeleteGroup, CanExecuteDeleteGroup);
			FilterList = new List<string>()
			{
				_allString,
				_groupsString,
				MeasuringKeys.CONTROL_SIGNAL,
				MeasuringKeys.ANALOG_MEASURING_ELEMENT,
				MeasuringKeys.DISCRET_MEASURING_ELEMENT,
				MeasuringKeys.DATE_TIME_ELEMENT,
			};
			SelectedFilterString = _allString;
			_measuringElementViewModelFactory = StaticContainer.Container.Resolve<IMeasuringElementViewModelFactory>();
		}

		public void UpdateMeasuringElements()
		{
			var groups= PresentationElementViewModels.Where(model => model.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel).ToList();
			foreach (var element in PresentationElementViewModels)
			{
				if (!(element.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel)&&!_positioningInfosDictionary.ContainsKey((element.TemplatedViewModelToShowOnCanvas as IUniqueId).Id))
				{
					_positioningInfosDictionary.Add((element.TemplatedViewModelToShowOnCanvas as IUniqueId).Id,element.PositioningInfoViewModel);
				}
			}
			PresentationElementViewModels.Clear();
			PresentationElementViewModels.AddCollection(groups);
		
			foreach (var measuringElementEditorViewModel in _measuringGroupEditorViewModel
				.MeasuringElementEditorViewModels)
			{

				var measuringElementViewModel = _measuringElementViewModelFactory.CreateMeasuringElementViewModel(
					new MeasuringElementSaver().SaveMeasuringElement(measuringElementEditorViewModel),
					_measuringGroupEditorViewModel.Header);

				var presentationElementViewModel = new PresentationElementViewModel(
					InitializeTestValue(measuringElementViewModel));

				InitializePositionInto(presentationElementViewModel, measuringElementViewModel);

				PresentationElementViewModels.Add(presentationElementViewModel);
			}
		}

		private void InitializePositionInto(PresentationElementViewModel presentationElementViewModel, IMeasuringElementViewModel measuringElementViewModel)
		{
			if (_positioningInfosDictionary.ContainsKey(measuringElementViewModel.Id))
			{
				presentationElementViewModel.PositioningInfoViewModel =
					_positioningInfosDictionary[measuringElementViewModel.Id];
			}
			else
			{
				_positioningInfosDictionary.Add(measuringElementViewModel.Id,presentationElementViewModel.PositioningInfoViewModel);
			}
		}


		private IMeasuringElementViewModel InitializeTestValue(IMeasuringElementViewModel measuringElementViewModel)
		{
			if (measuringElementViewModel is IAnalogMeasuringElementViewModel analogMeasuringElement)
			{
				var value = StaticContainer.Container.Resolve<INumericValueViewModel>();
				value.NumValue = "1000";
				analogMeasuringElement.FormattedValueViewModel = value;
			}
			return measuringElementViewModel;
		}

		private bool CanExecuteDeleteGroup()
		{
			return SelectedElementViewModel?.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel;
		}

		private void OnDeleteGroup()
		{
			PresentationElementViewModels.Remove(SelectedElementViewModel);
			SelectedElementViewModel = null;
		}

		private void OnAddGroup()
		{
			var newOne= new PresentationElementViewModel(new PresentationGroupViewModel());
			SelectedElementViewModel = newOne;
			PresentationElementViewModels.Add(newOne);
			UpdateFilter();
		}

		public List<PresentationElementViewModel> FilteredPresentationElementViewModels
		{
			get => _filteredPresentationElementViewModels;
			set
			{
				_filteredPresentationElementViewModels = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<PresentationElementViewModel> PresentationElementViewModels { get; }
		public ICommand AddGroupCommand { get; }
		public RelayCommand DeleteGroupCommand { get; }

		public string SelectedFilterString
		{
			get => _selectedFilterString;
			set
			{
				_selectedFilterString = value;
				RaisePropertyChanged();
				UpdateFilter();
			}
		}

		private void UpdateFilter()
		{
			if (SelectedFilterString == _allString)
			{
				FilteredPresentationElementViewModels = PresentationElementViewModels.ToList();
			}
			else if (SelectedFilterString == _groupsString)
			{
				FilteredPresentationElementViewModels = PresentationElementViewModels.Where(model => model.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel).ToList();
			}
			
		}

		public List<string> FilterList { get; }

		public PresentationElementViewModel SelectedElementViewModel
		{
			get => _selectedElementViewModel;
			set
			{
				if (_selectedElementViewModel != null) _selectedElementViewModel.IsSelected = false;
				_selectedElementViewModel = value;
				if (value != null) value.IsSelected = true;
				RaisePropertyChanged();
				DeleteGroupCommand?.RaiseCanExecuteChanged();
			}
		}
	}
	
}