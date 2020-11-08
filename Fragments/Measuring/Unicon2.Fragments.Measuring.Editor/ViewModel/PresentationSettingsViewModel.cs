using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Helpers;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.Subscriptions;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel
{
	public class PresentationSettingsViewModel : ViewModelBase
	{
		private readonly IMeasuringGroupEditorViewModel _measuringGroupEditorViewModel;
		private PresentationElementViewModel _selectedElementViewModel;
		private readonly string _allString = "All";
		private readonly string _groupsString = "Groups";
		private PositioningInfoViewModel _bufferPositioningInfoViewModel;
		private string _selectedFilterString;
		private List<PresentationElementViewModel> _filteredPresentationElementViewModels;
		private IMeasuringElementViewModelFactory _measuringElementViewModelFactory;
		private Dictionary<Guid, PositioningInfoViewModel> _positioningInfosDictionary;

		public PresentationSettingsViewModel(IMeasuringGroupEditorViewModel measuringGroupEditorViewModel,
			Dictionary<Guid, PositioningInfoViewModel> positioningInfosDictionary)
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
			CopySelectedPositionInfo = new RelayCommand(() =>
			{
				_bufferPositioningInfoViewModel = SelectedElementViewModel.PositioningInfoViewModel;
				(PasteSelectedPositionInfo as RelayCommand)?.RaiseCanExecuteChanged();
				(PasteOnlySizeSelectedPositionInfo as RelayCommand)?.RaiseCanExecuteChanged();
			});
			PasteSelectedPositionInfo=new RelayCommand(() =>
				{
					SelectedElementViewModel.PositioningInfoViewModel.OffsetLeft =
						_bufferPositioningInfoViewModel.OffsetLeft;
					SelectedElementViewModel.PositioningInfoViewModel.OffsetTop =
						_bufferPositioningInfoViewModel.OffsetTop;
					SelectedElementViewModel.PositioningInfoViewModel.SizeHeight =
						_bufferPositioningInfoViewModel.SizeHeight;
					SelectedElementViewModel.PositioningInfoViewModel.SizeWidth =
						_bufferPositioningInfoViewModel.SizeWidth;
				},() => _bufferPositioningInfoViewModel!=null);

			PasteOnlySizeSelectedPositionInfo = new RelayCommand(() =>
				{
					SelectedElementViewModel.PositioningInfoViewModel.SizeHeight =
						_bufferPositioningInfoViewModel.SizeHeight;
					SelectedElementViewModel.PositioningInfoViewModel.SizeWidth =
						_bufferPositioningInfoViewModel.SizeWidth;
				},() => _bufferPositioningInfoViewModel!=null);



			SelectElement=new RelayCommand<object>((o =>
			{
				if (o is PresentationElementViewModel presentationElementViewModel)
				{
					SelectedElementViewModel = presentationElementViewModel;
				}
			} ));
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
			UpdateFilter();
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
			else if (measuringElementViewModel is IDiscretMeasuringElementViewModel discretMeasuringElement)
			{
				var value = StaticContainer.Container.Resolve<IBoolValueViewModel>();
				discretMeasuringElement.FormattedValueViewModel = value;
			}
			else if (measuringElementViewModel is IDateTimeMeasuringElementViewModel dateTimeMeasuringElement)
			{
			    var now = DateTime.Now;
			    dateTimeMeasuringElement.Date = $"{now.Day}.{now.Month}.{(now.Year-2000)}";
			    dateTimeMeasuringElement.Time = $"{now.Hour}:{now.Minute}:{now.Second},{now.Millisecond.ToString().Substring(0,2)}";

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
			SelectedElementViewModel.PositioningInfoViewModel.Dispose();
			SelectedElementViewModel = null;
			UpdateFilter();
		}

		private void OnAddGroup()
		{
			var group = new PresentationGroupViewModel();
			var newOne= new PresentationElementViewModel(group);
			newOne.PositioningInfoViewModel = new PositioningInfoViewModel(0, 0, 200, 200,
				new PresentationPositionChangedSubscription(group, this));
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
				FilteredPresentationElementViewModels = PresentationElementViewModels
					.Where(model => model.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel).ToList();
			}
			else if (SelectedFilterString == MeasuringKeys.CONTROL_SIGNAL)
			{
				FilteredPresentationElementViewModels = PresentationElementViewModels
					.Where(model => model.TemplatedViewModelToShowOnCanvas is IControlSignalViewModel).ToList();
			}
			else if (SelectedFilterString == MeasuringKeys.ANALOG_MEASURING_ELEMENT)
			{
				FilteredPresentationElementViewModels = PresentationElementViewModels
					.Where(model => model.TemplatedViewModelToShowOnCanvas is IAnalogMeasuringElementViewModel).ToList();
			}
			else if (SelectedFilterString == MeasuringKeys.DISCRET_MEASURING_ELEMENT)
			{
				FilteredPresentationElementViewModels = PresentationElementViewModels
					.Where(model => model.TemplatedViewModelToShowOnCanvas is IDiscretMeasuringElementViewModel).ToList();
			}
			else if (SelectedFilterString == MeasuringKeys.DATE_TIME_ELEMENT)
			{
			    FilteredPresentationElementViewModels = PresentationElementViewModels
			        .Where(model => model.TemplatedViewModelToShowOnCanvas is IDateTimeMeasuringElementViewModel).ToList();
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
		public ICommand PasteSelectedPositionInfo { get; }
		public ICommand CopySelectedPositionInfo { get; }
		public ICommand PasteOnlySizeSelectedPositionInfo { get; }

		public ICommand SelectElement { get; }

	}
	
}