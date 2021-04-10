using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers;
using Unicon2.Fragments.Oscilliscope.Helpers;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.ViewModel.Journal
{
    public class OscilloscopeJournalViewModel : ViewModelBase, IOscilloscopeJournalViewModel, IUniconJournalViewModel
    {
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private readonly ILocalizerService _localizerService;
        private readonly Func<IBoolValue> _boolValueGettingFunc;
        private readonly ILoadingSequenceLoaderRegistry _loadingSequenceLoaderRegistry;
        private readonly IJournalLoaderProvider _journalLoaderProvider;
        private readonly OscillogramLoader _oscillogramLoader;
        private LoaderHooks _loaderHooks;

        public OscilloscopeJournalViewModel(IValueViewModelFactory valueViewModelFactory,
            ILocalizerService localizerService, Func<IBoolValue> boolValueGettingFunc,
            ILoadingSequenceLoaderRegistry loadingSequenceLoaderRegistry, IJournalLoaderProvider journalLoaderProvider,OscillogramLoader oscillogramLoader)
        {
            this._valueViewModelFactory = valueViewModelFactory;
            this._localizerService = localizerService;
            this._boolValueGettingFunc = boolValueGettingFunc;
            _loadingSequenceLoaderRegistry = loadingSequenceLoaderRegistry;
            _journalLoaderProvider = journalLoaderProvider;
            _oscillogramLoader = oscillogramLoader;
            this.LoadCommand = new RelayCommand(this.OnLoadExecute);
            this.SelectedRows = new List<int>();
            _loaderHooks = new LoaderHooks(() =>
            {
                Table = new DynamicDataTable(this.JournalParametersNameList, null, true);
                RaisePropertyChanged(nameof(this.Table));
                RaisePropertyChanged(nameof(this.JournalParametersNameList));

            }, list =>
            {
                List<IFormattedValueViewModel> formattedValueViewModels = new List<IFormattedValueViewModel>();
                int index = this.Table.GetCurrentValueCount();
                IBoolValue boolValue = this._boolValueGettingFunc();
                boolValue.BoolValueProperty = this._oscillogramLoader.TryGetOscillogram(index, out string s,_oscilloscopeViewModel,_oscilloscopeModel,_oscilloscopeViewModel.DeviceContext);
                formattedValueViewModels.Add(this._valueViewModelFactory.CreateFormattedValueViewModel(boolValue));
                formattedValueViewModels.AddRange(list.Select((formattedValue =>
                    this._valueViewModelFactory.CreateFormattedValueViewModel(formattedValue))).ToList());

                this.Table.AddFormattedValueViewModel(formattedValueViewModels);
            });
        }

        private async void OnLoadExecute()
        {
            var loader =
                _loadingSequenceLoaderRegistry.ResolveLoader(_uniconJournal.JournalLoadingSequence, DeviceContext);

            if (loader is IJournalSequenceInitialingFromParameters
                initialingFromParameters)
            {
                initialingFromParameters.Initialize(new OscilloscopeLoadingSequenceInitializingParameters()
                {
                    AddressOfReadyMark = (ushort)this._oscilloscopeModel.OscillogramLoadingParameters.GetReadyPointAddress(),
                    NumberOfReadyMarkPoints = this._oscilloscopeModel.OscillogramLoadingParameters.GetReadyPointNumberOfPoints(),
                });
            }
            await _journalLoaderProvider.GetJournalLoader(DeviceContext, _uniconJournal,_loaderHooks).Load();
        }


        private IUniconJournal _uniconJournal;
        private List<int> _selectedRows;
        private IOscilloscopeModel _oscilloscopeModel;
        private bool _canExecuteJournalLoading;
        private IOscilloscopeViewModel _oscilloscopeViewModel;

        public List<string> JournalParametersNameList { get; set; }
        public DynamicDataTable Table { get; set; }

        public ICommand LoadCommand { get; set; }

        public bool CanExecuteJournalLoading
        {
            get { return this._canExecuteJournalLoading; }
            set
            {
                this._canExecuteJournalLoading = value;
                this.RaisePropertyChanged();
            }
        }


        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE_JOURNAL +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

    
        public List<int> SelectedRows
        {
            get { return this._selectedRows; }
            set
            {
                this._selectedRows = value;
                this.RaisePropertyChanged();
            }
        }

        public void SetParentOscilloscopeModel(IOscilloscopeModel oscilloscopeModel,
            IOscilloscopeViewModel oscilloscopeViewModel)
        {
            this._oscilloscopeModel = oscilloscopeModel;
            this._oscilloscopeViewModel = oscilloscopeViewModel;

        }

        public DeviceContext DeviceContext { get; set; }
        public void Initialize(IDeviceFragment deviceFragment)
        {
            IUniconJournal uniconJournal = deviceFragment as IUniconJournal;
            this._uniconJournal = uniconJournal;
            this.JournalParametersNameList = new List<string>();
            this.JournalParametersNameList.Add(this._localizerService.GetLocalizedString(ApplicationGlobalNames.IS_LOADED));
            if (uniconJournal.RecordTemplate != null)
            {
                foreach (IJournalParameter journalParameter in uniconJournal.RecordTemplate.JournalParameters)
                {
                    if (journalParameter is IComplexJournalParameter)
                    {
                        foreach (ISubJournalParameter subJournalParameter in (journalParameter as IComplexJournalParameter)
                            .ChildJournalParameters)
                        {
                            this.JournalParametersNameList.Add(subJournalParameter.Name);
                        }
                    }
                    else
                    {
                        this.JournalParametersNameList.Add(journalParameter.Name);
                    }
                }
            }
            this.Table = new DynamicDataTable(this.JournalParametersNameList, null, true);
            this.RaisePropertyChanged(nameof(this.Table));
            this.RaisePropertyChanged(nameof(this.JournalParametersNameList));
        }
    }
}
