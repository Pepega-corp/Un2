using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.View;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel
{
    public class MeasuringMonitorEditorViewModel : ViewModelBase, IMeasuringMonitorEditorViewModel
    {
        private readonly ITypesContainer _container;
        private readonly IMeasuringGroupEditorViewModelFactory _measuringGroupEditorViewModelFactory;
        private IMeasuringMonitor _measuringMonitor;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private bool _isContextMenuOpen;
        private bool _isListViewSelected;

        public MeasuringMonitorEditorViewModel(ITypesContainer container,
            IMeasuringGroupEditorViewModelFactory measuringGroupEditorViewModelFactory,
            IMeasuringMonitor measuringMonitor, IApplicationGlobalCommands applicationGlobalCommands)
        {
            _container = container;
            _measuringGroupEditorViewModelFactory = measuringGroupEditorViewModelFactory;
            _measuringMonitor = measuringMonitor;
            _applicationGlobalCommands = applicationGlobalCommands;
            MeasuringGroupEditorViewModels = new ObservableCollection<IMeasuringGroupEditorViewModel>();
            AddMeasuringGroupCommand = new RelayCommand(OnAddMeasuringGroupExecute);
            DeleteGroupCommand = new RelayCommand<object>(OnDeleteGroupExecute);
            SetElementLeftCommand = new RelayCommand<object>(OnSetElementLeftExecute, CanExecuteSetElementLeft);
            SetElementRightCommand = new RelayCommand<object>(OnSetElementRightExecute, CanExecuteSetElementRight);
            CheckElementsPositionCommand = new RelayCommand(() =>
            {
                (SetElementLeftCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
                (SetElementRightCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();

            });
			var r=new GongSolutions.Wpf.DragDrop.DefaultDragHandler();
            OpenConfigurationSettingsCommand = new RelayCommand(OnOpenConfigurationSettingsExecute);
        }

        private void OnOpenConfigurationSettingsExecute()
        {
            IFragmentSettingsViewModel configurationSettingsViewModel =
                _container.Resolve<IFragmentSettingsViewModel>();
            if (_measuringMonitor.FragmentSettings == null)
            {
                _measuringMonitor.FragmentSettings = _container.Resolve<IFragmentSettings>();
            }
            configurationSettingsViewModel.Model = _measuringMonitor.FragmentSettings;
            _applicationGlobalCommands.ShowWindowModal(() => new MeasuringSettingsView(),
                configurationSettingsViewModel);
        }

        private bool CanExecuteSetElementRight(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return false;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            return (startIndex < MeasuringGroupEditorViewModels.Count - 1);
        }

        private bool CanExecuteSetElementLeft(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return false;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            return (startIndex > 0);
        }

        private void OnSetElementRightExecute(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            int finalIndex = startIndex + 1;
            MeasuringGroupEditorViewModels.Move(startIndex, finalIndex);
            CheckElementsPositionCommand?.Execute(null);
        }

        private void OnSetElementLeftExecute(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            int finalIndex = startIndex - 1;
            MeasuringGroupEditorViewModels.Move(startIndex, finalIndex);
            CheckElementsPositionCommand?.Execute(null);
        }

        private void OnDeleteGroupExecute(object obj)
        {
            if (!_applicationGlobalCommands.AskUserToDeleteSelectedGlobal(this)) return;
            if (obj is IMeasuringGroupEditorViewModel)
            {
                MeasuringGroupEditorViewModels.Remove(obj as IMeasuringGroupEditorViewModel);
            }
        }

        private void OnAddMeasuringGroupExecute()
        {
            MeasuringGroupEditorViewModels.Add(
                _measuringGroupEditorViewModelFactory.CreateMeasuringGroupEditorViewModel());
        }


        public string StrongName => MeasuringKeys.MEASURING_MONITOR +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

       
		

        public string NameForUiKey => MeasuringKeys.MEASURING_MONITOR;
        public IDeviceFragment BuildDeviceFragment()
        {
			_measuringMonitor.MeasuringGroups.Clear();
			foreach (IMeasuringGroupEditorViewModel measuringGroupEditorViewModel in MeasuringGroupEditorViewModels)
			{
				_measuringMonitor.MeasuringGroups.Add(measuringGroupEditorViewModel.Model as IMeasuringGroup);
			}
			return _measuringMonitor;
		}

        public ICommand OpenConfigurationSettingsCommand { get; }
        public ICommand AddMeasuringGroupCommand { get; }
        public ICommand SetElementLeftCommand { get; }
        public ICommand SetElementRightCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand CheckElementsPositionCommand { get; set; }


        public ObservableCollection<IMeasuringGroupEditorViewModel> MeasuringGroupEditorViewModels { get; set; }
        public void Initialize(IDeviceFragment deviceFragment)
        {
			_measuringMonitor = deviceFragment as IMeasuringMonitor;
			MeasuringGroupEditorViewModels.Clear();
			foreach (IMeasuringGroup measuringGroup in _measuringMonitor.MeasuringGroups)
			{
				MeasuringGroupEditorViewModels.Add(_measuringGroupEditorViewModelFactory
					.CreateMeasuringGroupEditorViewModel(measuringGroup));
			}
		}
    }
}