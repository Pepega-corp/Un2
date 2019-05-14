using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.EvenrArgs;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.ViewModel
{
    public class UniconJournalViewModel : ViewModelBase, IUniconJournalViewModel, IFragmentViewModel
    {
        private readonly ILocalizerService _localizerService;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly IValueViewModelFactory _valueViewModelFactory;


        //private readonly IJournalRecordViewModelFactory _journalRecordViewModelFactory;
        private List<string> _journalParametersNameList;
        private IUniconJournal _uniconJournal;
        private DynamicDataTable _table;
        private bool _canExecuteJournalLoading;

        public UniconJournalViewModel(ILocalizerService localizerService,
            IFragmentOptionsViewModel fragmentOptionsViewModel,
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelgetFunc,
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelgetFunc, IApplicationGlobalCommands applicationGlobalCommands, IValueViewModelFactory valueViewModelFactory)
        {
            this._localizerService = localizerService;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this._valueViewModelFactory = valueViewModelFactory;
            //_journalRecordViewModelFactory = journalRecordViewModelFactory;
            //JournalRecordViewModelCollection = new ObservableCollection<IJournalRecordViewModel>();

            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Device";
            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Load";
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);
            this.LoadCommand = new RelayCommand(this.OnLoadJournal, this.CanLoadExecute);
            fragmentOptionCommandViewModel.OptionCommand = this.LoadCommand;
            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);
            this.FragmentOptionsViewModel = fragmentOptionsViewModel;
            this.CanExecuteJournalLoading = true;
        }

        private bool CanLoadExecute()
        {
            return this.CanExecuteJournalLoading;
        }

        private async void OnLoadJournal()
        {
            try
            {
                await this._uniconJournal.Load();
            }
            catch (Exception e)
            {
                this._applicationGlobalCommands.ShowErrorMessage(ApplicationGlobalNames.ErrorMessages.JOURNAL_READING_ERROR, this);
                this._uniconJournal.JournalRecordsChanged?.Invoke(new RecordChangingEventArgs()
                {
                    RecordChangingEnum = RecordChangingEnum.RecordsReadingFinished
                });
            }

        }


        #region Implementation of IStronglyNamed

        public string StrongName => JournalKeys.UNICON_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion

        #region Implementation of IViewModel

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
                foreach (IJournalParameter journalParameter in uniconJournal.RecordTemplate.JournalParameters)
                {
                    if (journalParameter is IComplexJournalParameter)
                    {
                        foreach (ISubJournalParameter subJournalParameter in (journalParameter as IComplexJournalParameter).ChildJournalParameters)
                        {
                            this.JournalParametersNameList.Add(subJournalParameter.Name);
                        }
                    }
                    else
                    {
                        this.JournalParametersNameList.Add(journalParameter.Name);
                    }
                }
                this.Table = new DynamicDataTable(this.JournalParametersNameList, null, true);
                this.RaisePropertyChanged(nameof(this.Table));
                this.RaisePropertyChanged(nameof(this.JournalParametersNameList));
                this._uniconJournal.JournalRecordsChanged += (ea) =>
                {
                    if (ea.RecordChangingEnum == RecordChangingEnum.RecordAdded)
                    {
                        this.Table.AddFormattedValueViewModel(ea.JournalRecord.FormattedValues.Select((formattedValue => this._valueViewModelFactory.CreateFormattedValueViewModel(formattedValue))).ToList());
                    }
                    else if (ea.RecordChangingEnum == RecordChangingEnum.RecordsRefreshed)
                    {
                        this.Table = new DynamicDataTable(this.JournalParametersNameList, null, true);
                    }
                    else if (ea.RecordChangingEnum == RecordChangingEnum.RecordsReadingStarted)
                    {
                        this.CanExecuteJournalLoading = false;
                        (this.LoadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    }
                    else if (ea.RecordChangingEnum == RecordChangingEnum.RecordsReadingFinished)
                    {
                        this.CanExecuteJournalLoading = true;
                        (this.LoadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    }
                };
            }
        }

        #endregion

        #region Implementation of IUniconJournalViewModel

        public List<string> JournalParametersNameList
        {
            get { return this._journalParametersNameList; }
            set
            {
                this._journalParametersNameList = value;
                this.RaisePropertyChanged();
            }
        }

        public DynamicDataTable Table
        {
            get { return this._table; }
            set
            {
                this._table = value;
                this.RaisePropertyChanged();
            }
        }

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

        #endregion

        #region Implementation of IFragmentViewModel

        public string NameForUiKey => this._localizerService.GetLocalizedString(JournalKeys.UNICON_JOURNAL) + "(" + this._uniconJournal.Name + ")";
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        #endregion
    }
}
