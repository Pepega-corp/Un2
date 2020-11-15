using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.Commands;
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
            this._sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            this.SelectTestConnectionProperty = new RelayCommand(this.OnSelectTestConnectionExecute);
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmitExecute);
            this.CancelCommand = new RelayCommand<object>(this.OnCancelExecute);
            this.ExpectedValues = new ObservableCollection<StringWrapper>();
            this.DeleteExpectedValueCommand = new RelayCommand<object>(this.OnDeleteExpectedValueExecute);
            this.AddExpectedValueCommand = new RelayCommand(this.OnAddExpectedValueExecute);
            this.DefaultComPortConfigurationViewModel = comPortConfigurationViewModel;
        }

        private void OnAddExpectedValueExecute()
        {
            this.ExpectedValues.Add(new StringWrapper(""));
        }

        private void OnDeleteExpectedValueExecute(object obj)
        {
            if ((obj is StringWrapper) && (this.ExpectedValues.Contains(obj as StringWrapper)))
            {
                this.ExpectedValues.Remove(obj as StringWrapper);
            }
        }

        private void OnCancelExecute(object obj)
        {
            // this._model.DeviceValueContaining = this._previousDeviceValueContaining;
            this._model.DefaultComPortConfiguration = this._previousComPortConfiguration;
            this.CloseWindow(obj);
        }

        private void OnSubmitExecute(object obj)
        {
            this._model.ExpectedValues = this.ExpectedValues.Select((wrapper => wrapper.StringValue)).ToList();
            this._model.DefaultComPortConfiguration = this.DefaultComPortConfigurationViewModel.ComPortConfiguration as IComPortConfiguration;

            this.CloseWindow(obj);
        }


        private void CloseWindow(object window)
        {
            (window as Window)?.Close();
        }

        private void OnSelectTestConnectionExecute()
        {
            var res=
                this._sharedResourcesGlobalViewModel.OpenSharedResourcesForSelectingString<IPropertyEditorViewModel>();
          
             this._model.RelatedResourceString = res;
            this.RaisePropertyChanged(nameof(this.SelectedPropertyString));
        }


        public string StrongName => nameof(ConnectionStateViewModel);

        public object Model
        {
            get
            {

                return this._model;

            }
            set
            {
                this._model = value as IConnectionState;
                this.ExpectedValues.Clear();
                if (this._model.ExpectedValues == null)
                {
                    this._model.ExpectedValues = new List<string>();
                }
                this.ExpectedValues.AddCollection(this._model.ExpectedValues.Select(s => new StringWrapper(s)));
                //   this._previousDeviceValueContaining = this._model.DeviceValueContaining;
                if (this._model.DefaultComPortConfiguration != null)
                {
                    this._previousComPortConfiguration = this._model.DefaultComPortConfiguration.Clone() as IComPortConfiguration;
                    this.DefaultComPortConfigurationViewModel.ComPortConfiguration = this._model.DefaultComPortConfiguration;
                }
                this.RaisePropertyChanged(nameof(this.SelectedPropertyString));
            }
        }

        public ObservableCollection<StringWrapper> ExpectedValues { get; }

        public IComPortConfigurationViewModel DefaultComPortConfigurationViewModel { get; }

        public string SelectedPropertyString => this._model.RelatedResourceString;
        public ICommand SelectTestConnectionProperty { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteExpectedValueCommand { get; }
        public ICommand AddExpectedValueCommand { get; }
    }
}
