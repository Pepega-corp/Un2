using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.View;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Infrastructure;
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
            this._container = container;
            this._measuringGroupEditorViewModelFactory = measuringGroupEditorViewModelFactory;
            this._measuringMonitor = measuringMonitor;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this.MeasuringGroupEditorViewModels = new ObservableCollection<IMeasuringGroupEditorViewModel>();
            this.AddMeasuringGroupCommand = new RelayCommand(this.OnAddMeasuringGroupExecute);
            this.DeleteGroupCommand = new RelayCommand<object>(this.OnDeleteGroupExecute);
            this.SetElementLeftCommand = new RelayCommand<object>(this.OnSetElementLeftExecute, this.CanExecuteSetElementLeft);
            this.SetElementRightCommand = new RelayCommand<object>(this.OnSetElementRightExecute, this.CanExecuteSetElementRight);
            this.CheckElementsPositionCommand = new RelayCommand(() =>
            {
                (this.SetElementLeftCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
                (this.SetElementRightCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();

            });
            this.OpenConfigurationSettingsCommand = new RelayCommand(this.OnOpenConfigurationSettingsExecute);
        }

        private void OnOpenConfigurationSettingsExecute()
        {
            IFragmentSettingsViewModel configurationSettingsViewModel =
                this._container.Resolve<IFragmentSettingsViewModel>();
            if (this._measuringMonitor.FragmentSettings == null)
            {
                this._measuringMonitor.FragmentSettings = this._container.Resolve<IFragmentSettings>();
            }
            configurationSettingsViewModel.Model = this._measuringMonitor.FragmentSettings;
            this._applicationGlobalCommands.ShowWindowModal(() => new MeasuringSettingsView(),
                configurationSettingsViewModel);
        }

        private bool CanExecuteSetElementRight(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return false;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = this.MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            return (startIndex < this.MeasuringGroupEditorViewModels.Count - 1);
        }

        private bool CanExecuteSetElementLeft(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return false;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = this.MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            return (startIndex > 0);
        }

        private void OnSetElementRightExecute(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = this.MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            int finalIndex = startIndex + 1;
            this.MeasuringGroupEditorViewModels.Move(startIndex, finalIndex);
            this.CheckElementsPositionCommand?.Execute(null);
        }

        private void OnSetElementLeftExecute(object arg)
        {
            if (!(arg is IMeasuringGroupEditorViewModel)) return;
            IMeasuringGroupEditorViewModel elementToSet = arg as IMeasuringGroupEditorViewModel;
            int startIndex = this.MeasuringGroupEditorViewModels.IndexOf(elementToSet);
            int finalIndex = startIndex - 1;
            this.MeasuringGroupEditorViewModels.Move(startIndex, finalIndex);
            this.CheckElementsPositionCommand?.Execute(null);
        }

        private void OnDeleteGroupExecute(object obj)
        {
            if (!this._applicationGlobalCommands.AskUserToDeleteSelectedGlobal(this)) return;
            if (obj is IMeasuringGroupEditorViewModel)
            {
                this.MeasuringGroupEditorViewModels.Remove(obj as IMeasuringGroupEditorViewModel);
            }
        }

        private void OnAddMeasuringGroupExecute()
        {
            this.MeasuringGroupEditorViewModels.Add(
                this._measuringGroupEditorViewModelFactory.CreateMeasuringGroupEditorViewModel());
        }


        #region Implementation of IStronglyNamed

        public string StrongName => MeasuringKeys.MEASURING_MONITOR +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object model)
        {
            this._measuringMonitor = model as IMeasuringMonitor;
            this.MeasuringGroupEditorViewModels.Clear();
            foreach (IMeasuringGroup measuringGroup in this._measuringMonitor.MeasuringGroups)
            {
                this.MeasuringGroupEditorViewModels.Add(this._measuringGroupEditorViewModelFactory
                    .CreateMeasuringGroupEditorViewModel(measuringGroup));
            }
        }


        private IMeasuringMonitor GetModel()
        {
            this._measuringMonitor.MeasuringGroups.Clear();
            foreach (IMeasuringGroupEditorViewModel measuringGroupEditorViewModel in this.MeasuringGroupEditorViewModels)
            {
                this._measuringMonitor.MeasuringGroups.Add(measuringGroupEditorViewModel.Model as IMeasuringGroup);
            }
            return this._measuringMonitor;
        }


        #endregion

        #region Implementation of IFragmentEditorViewModel

        public string NameForUiKey => MeasuringKeys.MEASURING_MONITOR;

        #endregion

        #region Implementation of IMeasuringMonitorEditorViewModel

        public ICommand OpenConfigurationSettingsCommand { get; }
        public ICommand AddMeasuringGroupCommand { get; }
        public ICommand SetElementLeftCommand { get; }
        public ICommand SetElementRightCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand CheckElementsPositionCommand { get; set; }


        public ObservableCollection<IMeasuringGroupEditorViewModel> MeasuringGroupEditorViewModels { get; set; }


        #endregion
    }
}