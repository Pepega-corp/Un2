using Unicon2.Fragments.Journals.Editor.Interfaces.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.ViewModel.LoadingSequence
{
    public class OffsetLoadingSequenceEditorViewModel : ViewModelBase, IOffsetLoadingSequenceEditorViewModel
    {
        private int _numberOfRecords;
        private ushort _journalStartAddress;
        private ushort _numberOfPointsInRecord;
        private ushort _wordFormatFrom;
        private ushort _wordFormatTo;
        private bool _isWordFormatNotForTheWholeRecord;
        private IOffsetLoadingSequence _offsetLoadingSequence;

        public string StrongName => JournalKeys.OFFSET_LOADING_SEQUENCE +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model
        {
            get
            {
                this._offsetLoadingSequence.IsWordFormatNotForTheWholeRecord = this.IsWordFormatNotForTheWholeRecord;
                this._offsetLoadingSequence.JournalStartAddress = this.JournalStartAddress;
                this._offsetLoadingSequence.NumberOfPointsInRecord = this.NumberOfPointsInRecord;
                this._offsetLoadingSequence.NumberOfRecords = this.NumberOfRecords;
                this._offsetLoadingSequence.WordFormatTo = this.WordFormatTo;
                this._offsetLoadingSequence.WordFormatFrom = this.WordFormatFrom;
                return this._offsetLoadingSequence;
            }
            set
            {
                this._offsetLoadingSequence = value as IOffsetLoadingSequence;
                this.IsWordFormatNotForTheWholeRecord = this._offsetLoadingSequence.IsWordFormatNotForTheWholeRecord;
                this.JournalStartAddress = this._offsetLoadingSequence.JournalStartAddress;
                this.NumberOfPointsInRecord = this._offsetLoadingSequence.NumberOfPointsInRecord;
                this.NumberOfRecords = this._offsetLoadingSequence.NumberOfRecords;
                this.WordFormatTo = this._offsetLoadingSequence.WordFormatTo;
                this.WordFormatFrom = this._offsetLoadingSequence.WordFormatFrom;
            }
        }

        public int NumberOfRecords
        {
            get { return this._numberOfRecords; }
            set
            {
                this._numberOfRecords = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort JournalStartAddress
        {
            get { return this._journalStartAddress; }
            set
            {
                this._journalStartAddress = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort NumberOfPointsInRecord
        {
            get { return this._numberOfPointsInRecord; }
            set
            {
                this._numberOfPointsInRecord = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort WordFormatFrom
        {
            get { return this._wordFormatFrom; }
            set
            {
                this._wordFormatFrom = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort WordFormatTo
        {
            get { return this._wordFormatTo; }
            set
            {
                this._wordFormatTo = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsWordFormatNotForTheWholeRecord
        {
            get { return this._isWordFormatNotForTheWholeRecord; }
            set
            {
                this._isWordFormatNotForTheWholeRecord = value;
                this.RaisePropertyChanged();
            }
        }

        public string NameForUiKey => this._offsetLoadingSequence.StrongName;
    }
}
