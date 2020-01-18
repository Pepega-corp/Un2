using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.EvenrArgs;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.ViewModel.Journal
{
    public class OscilloscopeJournalViewModel : ViewModelBase, IOscilloscopeJournalViewModel
    {
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private readonly ILocalizerService _localizerService;
        private readonly Func<IBoolValue> _boolValueGettingFunc;

        public OscilloscopeJournalViewModel(IValueViewModelFactory valueViewModelFactory, ILocalizerService localizerService, Func<IBoolValue> boolValueGettingFunc)
        {
            this._valueViewModelFactory = valueViewModelFactory;
            this._localizerService = localizerService;
            this._boolValueGettingFunc = boolValueGettingFunc;
            this.LoadCommand = new RelayCommand(this.OnLoadExecute);
            this.SelectedRows = new List<int>();
        }

        private async void OnLoadExecute()
        {
            this._uniconJournal.JournalLoadingSequence.Initialize(new OscilloscopeLoadingSequenceInitializingParameters()
            {
                AddressOfReadyMark = (ushort)this._oscilloscopeModel.OscillogramLoadingParameters.GetReadyPointAddress(),
                NumberOfReadyMarkPoints = this._oscilloscopeModel.OscillogramLoadingParameters.GetReadyPointNumberOfPoints(),
            });
            await this._uniconJournal.Load();
        }


        private IUniconJournal _uniconJournal;
        private List<int> _selectedRows;
        private IOscilloscopeModel _oscilloscopeModel;
        private bool _canExecuteJournalLoading;

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

        public object Model
        {
            get
            {
                return this._uniconJournal;
            }
            set
            {
                IUniconJournal uniconJournal = value as IUniconJournal;
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
                this._uniconJournal.JournalRecordsChanged += (ea) =>
                {
                    if (ea.RecordChangingEnum == RecordChangingEnum.RecordAdded)
                    {
                        List<IFormattedValueViewModel> formattedValueViewModels = new List<IFormattedValueViewModel>();
                        int index = this.Table.GetCurrentValueCount();
                        IBoolValue boolValue = this._boolValueGettingFunc();
                        boolValue.BoolValueProperty = this._oscilloscopeModel.TryGetOscillogram(index, out string s);
                        formattedValueViewModels.Add(this._valueViewModelFactory.CreateFormattedValueViewModel(boolValue));
                        formattedValueViewModels.AddRange(ea.JournalRecord.FormattedValues.Select((formattedValue => this._valueViewModelFactory.CreateFormattedValueViewModel(formattedValue))).ToList());

                        this.Table.AddFormattedValueViewModel(formattedValueViewModels);
                    }
                    else if (ea.RecordChangingEnum == RecordChangingEnum.RecordsRefreshed)
                    {
                        this.Table = new DynamicDataTable(this.JournalParametersNameList, null, true);
                        this.RaisePropertyChanged(nameof(this.Table));
                        this.RaisePropertyChanged(nameof(this.JournalParametersNameList));
                    }
                    else if (ea.RecordChangingEnum == RecordChangingEnum.RecordsReadingStarted)
                    {
                        (this.LoadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    }
                    else if (ea.RecordChangingEnum == RecordChangingEnum.RecordsReadingFinished)
                    {
                        (this.LoadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    }
                };
            }
        }

        public List<int> SelectedRows
        {
            get { return this._selectedRows; }
            set
            {
                this._selectedRows = value;
                this.RaisePropertyChanged();
            }
        }

        public void SetParentOscilloscopeModel(IOscilloscopeModel oscilloscopeModel)
        {
            this._oscilloscopeModel = oscilloscopeModel;
        }
    }
}
