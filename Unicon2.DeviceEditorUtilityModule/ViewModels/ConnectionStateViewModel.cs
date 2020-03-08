using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ConnectionStateViewModel : ViewModelBase, IConnectionStateViewModel
    {
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private IConnectionState _model;
        //private IDeviceValueContaining _previousDeviceValueContaining;
        private IComPortConfiguration _previousComPortConfiguration;

        public ConnectionStateViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel, IComPortConfigurationViewModel comPortConfigurationViewModel)
        {
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            SelectTestConnectionProperty = new RelayCommand(OnSelectTestConnectionExecute);
            SubmitCommand = new RelayCommand<object>(OnSubmitExecute);
            CancelCommand = new RelayCommand<object>(OnCancelExecute);
            ExpectedValues = new ObservableCollection<StringWrapper>();
            DeleteExpectedValueCommand = new RelayCommand<object>(OnDeleteExpectedValueExecute);
            AddExpectedValueCommand = new RelayCommand(OnAddExpectedValueExecute);
            DefaultComPortConfigurationViewModel = comPortConfigurationViewModel;
        }

        private void OnAddExpectedValueExecute()
        {
            ExpectedValues.Add(new StringWrapper(""));
        }

        private void OnDeleteExpectedValueExecute(object obj)
        {
            if ((obj is StringWrapper) && (ExpectedValues.Contains(obj as StringWrapper)))
            {
                ExpectedValues.Remove(obj as StringWrapper);
            }
        }

        private void OnCancelExecute(object obj)
        {
           // this._model.DeviceValueContaining = this._previousDeviceValueContaining;
            _model.DefaultComPortConfiguration = _previousComPortConfiguration;
            CloseWindow(obj);
        }

        private void OnSubmitExecute(object obj)
        {
            _model.ExpectedValues = ExpectedValues.Select((wrapper => wrapper.StringValue)).ToList();
            _model.DefaultComPortConfiguration = DefaultComPortConfigurationViewModel.Model as IComPortConfiguration;

            CloseWindow(obj);
        }


        private void CloseWindow(object window)
        {
            (window as Window)?.Close();
        }

        private void OnSelectTestConnectionExecute()
        {
            IUshortFormattable ushortFormattable =
                _sharedResourcesGlobalViewModel.OpenSharedResourcesForSelecting<IUshortFormattable>();
            if (ushortFormattable == null) return;
           // this._model.DeviceValueContaining = ushortFormattable as IDeviceValueContaining;
            RaisePropertyChanged(nameof(SelectedPropertyString));
        }


        public string StrongName => nameof(ConnectionStateViewModel);

        public object Model
        {
            get
            {

                return _model;

            }
            set
            {
                _model = value as IConnectionState;
                ExpectedValues.Clear();
                if (_model.ExpectedValues == null)
                {
                    _model.ExpectedValues = new List<string>();
                }
                ExpectedValues.AddCollection(_model.ExpectedValues.Select(s => new StringWrapper(s)));
             //   this._previousDeviceValueContaining = this._model.DeviceValueContaining;
                if (_model.DefaultComPortConfiguration != null)
                {
                    _previousComPortConfiguration = _model.DefaultComPortConfiguration.Clone() as IComPortConfiguration;
                    DefaultComPortConfigurationViewModel.Model = _model.DefaultComPortConfiguration;
                }
                RaisePropertyChanged(nameof(SelectedPropertyString));
            }
        }

        public ObservableCollection<StringWrapper> ExpectedValues { get; }

        public IComPortConfigurationViewModel DefaultComPortConfigurationViewModel { get; }

        public string SelectedPropertyString => _model.DeviceValueContaining?.Name;
        public ICommand SelectTestConnectionProperty { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteExpectedValueCommand { get; }
        public ICommand AddExpectedValueCommand { get; }
    }
}
