using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.ViewModel
{
    public class UniconJournalEditorViewModel : ViewModelBase, IUniconJournalEditorViewModel
    {
        private IUniconJournal _uniconJournal;

        private readonly IJournalSequenceEditorViewModelFactory _journalSequenceEditorViewModelFactory;
        private string _name;
        private IJournalLoadingSequenceEditorViewModel _selectedJournalLoadingSequenceEditorViewModel;
        private ObservableCollection<IJournalLoadingSequenceEditorViewModel> _journalLoadingSequenceEditorViewModels;
        private IRecordTemplateEditorViewModel _journalRecordTemplateEditorViewModel;

        public UniconJournalEditorViewModel(IUniconJournal uniconJournal,
            IJournalSequenceEditorViewModelFactory journalSequenceEditorViewModelFactory,
            IRecordTemplateEditorViewModel recordTemplateEditorViewModel)
        {
            this._uniconJournal = uniconJournal;

            this._journalSequenceEditorViewModelFactory = journalSequenceEditorViewModelFactory;
            this.JournalLoadingSequenceEditorViewModels = 
                new ObservableCollection<IJournalLoadingSequenceEditorViewModel>(this._journalSequenceEditorViewModelFactory.GetAvailableLoadingSequenceEditorViewModels());
            this.JournalRecordTemplateEditorViewModel = recordTemplateEditorViewModel;
        }

        public ObservableCollection<IJournalLoadingSequenceEditorViewModel> JournalLoadingSequenceEditorViewModels
        {
            get { return this._journalLoadingSequenceEditorViewModels; }
            set
            {
                this._journalLoadingSequenceEditorViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public IJournalLoadingSequenceEditorViewModel SelectedJournalLoadingSequenceEditorViewModel
        {
            get { return this._selectedJournalLoadingSequenceEditorViewModel; }
            set
            {
                this._selectedJournalLoadingSequenceEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public IRecordTemplateEditorViewModel JournalRecordTemplateEditorViewModel
        {
            get { return this._journalRecordTemplateEditorViewModel; }
            set
            {
                this._journalRecordTemplateEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public string StrongName => JournalKeys.UNICON_JOURNAL +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

      

        public string NameForUiKey => JournalKeys.UNICON_JOURNAL;
        public IDeviceFragment BuildDeviceFragment()
        {
            this._uniconJournal.Name = this.Name;
            this._uniconJournal.RecordTemplate = this.JournalRecordTemplateEditorViewModel.Model as IRecordTemplate;

            if (this.SelectedJournalLoadingSequenceEditorViewModel != null)
            {
                this._uniconJournal.JournalLoadingSequence = this.SelectedJournalLoadingSequenceEditorViewModel.Model as IJournalLoadingSequence;
            }
            return this._uniconJournal;
        }

        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public async Task<Result> Initialize(IDeviceFragment deviceFragment)
        {
            IUniconJournal settingJournal = deviceFragment as IUniconJournal;
            if (deviceFragment == null) return Result.Create(false);
            this._uniconJournal = settingJournal;
            this.Name = settingJournal.Name;
            this.JournalRecordTemplateEditorViewModel.Model = this._uniconJournal.RecordTemplate;
            if (this._uniconJournal.JournalLoadingSequence != null)
            {
                this.SelectedJournalLoadingSequenceEditorViewModel = this.JournalLoadingSequenceEditorViewModels
                    .First((model => model.StrongName.Contains(this._uniconJournal.JournalLoadingSequence.StrongName)));
                this.SelectedJournalLoadingSequenceEditorViewModel.Model = this._uniconJournal.JournalLoadingSequence;
            }
            return Result.Create(true);
        }
    }
}
