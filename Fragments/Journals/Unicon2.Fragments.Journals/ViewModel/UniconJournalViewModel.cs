using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Journals.MemoryAccess;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.Behaviors;
using Unicon2.SharedResources.Icons;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.ViewModel
{
    public class UniconJournalViewModel : ViewModelBase, IUniconJournalViewModel, IFragmentViewModel
    {
        private readonly ILocalizerService _localizerService;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private List<string> _journalParametersNameList;
        private IUniconJournal _uniconJournal;
        private DynamicDataTable _table;
        private bool _canExecuteJournalLoading;
        private IDeviceEventsDispatcher _deviceEventsDispatcher;
        private IDeviceMemory _deviceMemory;

        public UniconJournalViewModel(ILocalizerService localizerService,
            IFragmentOptionsViewModel fragmentOptionsViewModel,
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelgetFunc,
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelgetFunc,
            IApplicationGlobalCommands applicationGlobalCommands)
        {
            _localizerService = localizerService;
            _applicationGlobalCommands = applicationGlobalCommands;
            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Device";
            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Load";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxIn;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);
            LoadCommand = new RelayCommand(OnLoadJournal, CanLoadExecute);
            fragmentOptionCommandViewModel.OptionCommand = LoadCommand;
            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);
            FragmentOptionsViewModel = fragmentOptionsViewModel;
            CanExecuteJournalLoading = true;
        }

        private bool CanLoadExecute()
        {
            return CanExecuteJournalLoading;
        }

        private async void OnLoadJournal()
        {
            try
            {
                await new JournalLoader(this, this.DeviceContext.DataProviderContaining, _uniconJournal).Load();
            }
            catch (Exception e)
            {
                _applicationGlobalCommands.ShowErrorMessage(e.ToString(), this);
                _applicationGlobalCommands.ShowErrorMessage(ApplicationGlobalNames.StatusMessages.JOURNAL_READING_ERROR,
                    this);
            }

        }


        public string StrongName =>
            JournalKeys.UNICON_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public List<string> JournalParametersNameList
        {
            get { return _journalParametersNameList; }
            set
            {
                _journalParametersNameList = value;
                RaisePropertyChanged();
            }
        }

        public DynamicDataTable Table
        {
            get { return _table; }
            set
            {
                _table = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadCommand { get; set; }

        public bool CanExecuteJournalLoading
        {
            get { return _canExecuteJournalLoading; }
            set
            {
                _canExecuteJournalLoading = value;
                RaisePropertyChanged();
            }
        }

        public string NameForUiKey => _localizerService.GetLocalizedString(JournalKeys.UNICON_JOURNAL) + "(" +
                                      _uniconJournal.Name + ")";

        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            IUniconJournal uniconJournal = deviceFragment as IUniconJournal;
            _uniconJournal = uniconJournal;
            JournalParametersNameList = new List<string>();
            foreach (IJournalParameter journalParameter in uniconJournal.RecordTemplate.JournalParameters)
            {
                if (journalParameter is IComplexJournalParameter)
                {
                    foreach (ISubJournalParameter subJournalParameter in (journalParameter as IComplexJournalParameter)
                        .ChildJournalParameters)
                    {
                        JournalParametersNameList.Add(subJournalParameter.Name);
                    }
                }
                else
                {
                    JournalParametersNameList.Add(journalParameter.Name);
                }
            }

            Table = new DynamicDataTable(JournalParametersNameList, null, true);
            RaisePropertyChanged(nameof(Table));
            RaisePropertyChanged(nameof(JournalParametersNameList));
        }

        public DeviceContext DeviceContext { get; set; }
    }
}