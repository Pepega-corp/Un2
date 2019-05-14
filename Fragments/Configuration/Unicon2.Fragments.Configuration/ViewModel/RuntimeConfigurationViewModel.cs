using System;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeConfigurationViewModel : ViewModelBase, IRuntimeConfigurationViewModel
    {
        private IDeviceConfiguration _deviceConfiguration;
        private readonly ITypesContainer _container;
        private readonly IRuntimeConfigurationItemViewModelFactory _runtimeConfigurationItemViewModelFactory;
        public RuntimeConfigurationViewModel(ITypesContainer container, IRuntimeConfigurationItemViewModelFactory runtimeConfigurationItemViewModelFactory)
        {
            this._container = container;
            this._runtimeConfigurationItemViewModelFactory = runtimeConfigurationItemViewModelFactory;
            this.AllRows = new ObservableCollection<IRuntimeConfigurationItemViewModel>();
            this.FragmentOptionsViewModel = (new ConfigurationOptionsHelper()).CreateConfigurationFragmentOptionsViewModel(this, this._container);
            this.RootConfigurationItemViewModels = new ObservableCollection<IRuntimeConfigurationItemViewModel>();
        }

        private ObservableCollection<IRuntimeConfigurationItemViewModel> _allRows;
        private IFragmentOptionsViewModel _fragmentOptionsViewModel;
        private ObservableCollection<IRuntimeConfigurationItemViewModel> _rootConfigurationItemViewModels;


        public ObservableCollection<IRuntimeConfigurationItemViewModel> RootConfigurationItemViewModels
        {
            get { return this._rootConfigurationItemViewModels; }
            set
            {
                this._rootConfigurationItemViewModels = value;
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

        #region Implementation of IStronglyNamed

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.RUNTIME_CONFIGURATION_VIEWMODEL;

        public string NameForUiKey => this._deviceConfiguration.StrongName;


        public IFragmentOptionsViewModel FragmentOptionsViewModel
        {
            get { return this._fragmentOptionsViewModel; }
            set
            {
                this._fragmentOptionsViewModel = value;
                this.RaisePropertyChanged();
            }
        }


        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this._deviceConfiguration; }
            set
            {
                if (!(value is IDeviceConfiguration)) throw new ArgumentException();
                this.SetModel(value as IDeviceConfiguration);

            }
        }


        private void SetModel(IDeviceConfiguration deviceConfiguration)
        {
            if (this._deviceConfiguration == deviceConfiguration) return;
            this._deviceConfiguration?.Dispose();
            this.AllRows.Clear();
            this.RootConfigurationItemViewModels.Clear();
            this._deviceConfiguration = deviceConfiguration;
            if (!this._deviceConfiguration.IsInitialized)
            {
                this._deviceConfiguration.InitializeFromContainer(this._container);
            }
            if (this._deviceConfiguration.RootConfigurationItemList != null)
            {
                foreach (IConfigurationItem configurationItem in this._deviceConfiguration.RootConfigurationItemList)
                {
                    this.RootConfigurationItemViewModels.Add(this._runtimeConfigurationItemViewModelFactory
                        .CreateRuntimeConfigurationItemViewModel(configurationItem));

                }
            }
            this.AllRows.AddCollection(this.RootConfigurationItemViewModels);
        }
        #endregion
    }
}