using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using Unicon2.Fragments.Oscilliscope.Helpers;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel;
using Unicon2.Fragments.Oscilliscope.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Progress;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Icons;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.ViewModel
{
    public class OscilloscopeViewModel : ViewModelBase, IOscilloscopeViewModel
    {
        private readonly ITypesContainer _container;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly OscillogramLoader _oscillogramLoader;
        private IOscilloscopeModel _oscilloscopeModel;
        private int _maxLoadingProgress;
        private int _currentLoadingProgress;
        private bool _isBusy;
        private CancellationTokenSource _loadingCancellationTokenSource;



        public OscilloscopeViewModel(ITypesContainer container, IApplicationGlobalCommands applicationGlobalCommands, IFragmentOptionsViewModel fragmentOptionsViewModel,
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelgetFunc,
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelgetFunc
            ,OscillogramLoader oscillogramLoader)
        {
            this._container = container;
            this._applicationGlobalCommands = applicationGlobalCommands;
            _oscillogramLoader = oscillogramLoader;
            this.OscilloscopeJournalViewModel = this._container.Resolve<IOscilloscopeJournalViewModel>();
            this.LoadSelectedOscillogramsCommand = new RelayCommand(this.OnLoadSelectedOscillogramsExecute);
            this.ShowOscillogramCommand = new RelayCommand<object>(this.OnShowOscillogramExecute);
            this.MaxLoadingProgress = 1;
            this.CurrentLoadingProgress = 0;
            this._loadingCancellationTokenSource = new CancellationTokenSource();
            this.StopLoadingCommand = new RelayCommand(this.OnStopLoadingExecute);

            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = OscilloscopeKeys.OSCILLOSCOPE_JOURNAL;
            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Load";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxIn;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);
            fragmentOptionCommandViewModel.OptionCommand = this.OscilloscopeJournalViewModel.LoadCommand;

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = OscilloscopeKeys.LOAD_SELECTED_OSCILLOGRAMS;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscDownload;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);
            fragmentOptionCommandViewModel.OptionCommand = this.LoadSelectedOscillogramsCommand;

            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);
            this.FragmentOptionsViewModel = fragmentOptionsViewModel;
            Oscillograms=new List<Oscillogram>();
        }


        private void OnStopLoadingExecute()
        {
            this._loadingCancellationTokenSource.Cancel();
        }

        private void OnShowOscillogramExecute(object o)
        {
            List<IFormattedValueViewModel> formattedValueViewModels = o as List<IFormattedValueViewModel>;
            int index = int.Parse((formattedValueViewModels[0] as INumericValueViewModel).NumValue) - 1;
            string oscPath = string.Empty;
            if (_oscillogramLoader.TryGetOscillogram(index, out oscPath,this,_oscilloscopeModel,DeviceContext))
            {
                this._applicationGlobalCommands.OpenOscillogram(oscPath);
            }
            else
            {
                this._applicationGlobalCommands.ShowErrorMessage(OscilloscopeKeys.OSCILLOGRAM_NOT_LOADED, this);
            }
        }

        private async void OnLoadSelectedOscillogramsExecute()
        {
            try
            {
                this.OscilloscopeJournalViewModel.CanExecuteJournalLoading = false;
                this._loadingCancellationTokenSource = new CancellationTokenSource();
                if (this.OscilloscopeJournalViewModel.SelectedRows.Count == 0)
                {
                    return;
                }
                IProgress<ITaskProgressReport> progressIndicator = new Progress<ITaskProgressReport>(this.OnProgressChanged);
               await _oscillogramLoader.LoadOscillogramsByNumber(this.OscilloscopeJournalViewModel.SelectedRows,
                    progressIndicator, this._loadingCancellationTokenSource.Token, _oscilloscopeModel, this,DeviceContext);
                this.OscilloscopeJournalViewModel.LoadCommand?.Execute(null);
            }
            finally
            {
                this.OscilloscopeJournalViewModel.CanExecuteJournalLoading = true;
            }
        }

        private void OnProgressChanged(ITaskProgressReport taskProgressReport)
        {
            this.MaxLoadingProgress = taskProgressReport.TotalProgressAmount;
            this.CurrentLoadingProgress = taskProgressReport.CurrentProgressAmount;
        }


        public IOscilloscopeJournalViewModel OscilloscopeJournalViewModel { get; set; }
        public ICommand LoadSelectedOscillogramsCommand { get; }

        public int MaxLoadingProgress
        {
            get { return this._maxLoadingProgress; }
            set
            {
                if (this._maxLoadingProgress == value) return;
                this._maxLoadingProgress = value;
                this.RaisePropertyChanged();
            }
        }

        public int CurrentLoadingProgress
        {
            get { return this._currentLoadingProgress; }
            set
            {
                this._currentLoadingProgress = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand ShowOscillogramCommand { get; }
        public ICommand StopLoadingCommand { get; }
        public List<Oscillogram> Oscillograms { get; }


        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        
        public string NameForUiKey => OscilloscopeKeys.OSCILLOSCOPE;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        public void Initialize(IDeviceFragment deviceFragment)
        {
            this._oscilloscopeModel = deviceFragment as IOscilloscopeModel;
            this.OscilloscopeJournalViewModel.Initialize(this._oscilloscopeModel.OscilloscopeJournal);
            this.OscilloscopeJournalViewModel.SetParentOscilloscopeModel(this._oscilloscopeModel,this);
        }

        public DeviceContext DeviceContext { get; set; }
    }
}
