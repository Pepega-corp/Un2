using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.Editor.ViewModel.LoadingSequence
{
    public class OscilloscopeJournalLoadingSequenceEditorViewModel : ViewModelBase, IOscilloscopeJournalLoadingSequenceEditorViewModel
    {
        private IOscilloscopeJournalLoadingSequence _oscilloscopeJournalLoadingSequence;
        private ushort _addressOfRecord;
        private ushort _numberOfPointsInRecord;
        
        #region Implementation of IStronglyNamed

        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            this._oscilloscopeJournalLoadingSequence = value as IOscilloscopeJournalLoadingSequence;
            this.AddressOfRecord = this._oscilloscopeJournalLoadingSequence.AddressOfRecord;
            this.NumberOfPointsInRecord = this._oscilloscopeJournalLoadingSequence.NumberOfPointsInRecord;
        }

        private IOscilloscopeJournalLoadingSequence GetModel()
        {
            this._oscilloscopeJournalLoadingSequence.AddressOfRecord = this.AddressOfRecord;
            this._oscilloscopeJournalLoadingSequence.NumberOfPointsInRecord = this.NumberOfPointsInRecord;
            return this._oscilloscopeJournalLoadingSequence;
        }

        #endregion

        #region Implementation of IJournalLoadingSequenceEditorViewModel

        public string NameForUiKey => OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE;

        #endregion

        #region Implementation of IOscilloscopeJournalLoadingSequenceEditorViewModel

        public ushort AddressOfRecord
        {
            get { return this._addressOfRecord; }
            set
            {
                this._addressOfRecord = value;
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

        #endregion
    }
}
