using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.Editor.ViewModel.OscillogramLoadingParameters
{
    public class OscilloscopeTagEditorViewModel : ViewModelBase, IOscilloscopeTagEditorViewModel
    {
        private IOscilloscopeTag _oscilloscopeTag;
        private string _selectedTag;
        private string _selectedJournalParameter;
        private List<IJournalParameter> _journalParametersModels;

        public OscilloscopeTagEditorViewModel()
        {
            this.AvailableJournalParameters = new ObservableCollection<string>();
            this.AvailableTags = new ObservableCollection<string>();
        }

        public string StrongName => nameof(OscilloscopeTagEditorViewModel);

        public object Model
        {
            get
            {
                this._oscilloscopeTag.RelatedJournalParameter = this._journalParametersModels.First((parameter => parameter.Name == this._selectedJournalParameter));
                this._oscilloscopeTag.TagKey = this._selectedTag;
                return this._oscilloscopeTag;
            }
            set
            {
                this._oscilloscopeTag = value as IOscilloscopeTag;
                this._selectedTag = this._oscilloscopeTag.TagKey;
                if (this._oscilloscopeTag.RelatedJournalParameter != null)
                {
                    this._selectedJournalParameter = this._oscilloscopeTag.RelatedJournalParameter.Name;
                }
            }
        }

        public void SetAvailableOptions(List<IJournalParameter> journalParameters, List<string> tags)
        {
            if(journalParameters==null)return;
            this._journalParametersModels = journalParameters;
            string bufParameter = null;
            if (this.SelectedJournalParameter != null)
            {
                bufParameter = this.AvailableJournalParameters.FirstOrDefault((s => s == this.SelectedJournalParameter));
            }
            this.AvailableJournalParameters.Clear();
            this.AvailableJournalParameters.AddCollection(journalParameters.Select((parameter => parameter.Name)));
            if (bufParameter != null)
            {
                this.SelectedJournalParameter = this.AvailableJournalParameters.FirstOrDefault((s => s == bufParameter));
            }
            RaisePropertyChanged(nameof(this.AvailableJournalParameters));
            string bufTag = null;
            if (this.SelectedTag != null)
            {
                bufTag = this.AvailableTags.FirstOrDefault((s => s == this.SelectedTag));
            }
            this.AvailableTags.Clear();
            this.AvailableTags.AddCollection(tags);
            if (bufTag != null)
            {
                this.SelectedTag = this.AvailableTags.FirstOrDefault((s => s == bufTag));
            }

            RaisePropertyChanged(nameof(this.AvailableTags));

        }

        public ObservableCollection<string> AvailableJournalParameters { get; }

        public ObservableCollection<string> AvailableTags { get; }

        public string SelectedTag
        {
            get { return this._selectedTag; }
            set
            {
                this._selectedTag = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedJournalParameter
        {
            get { return this._selectedJournalParameter; }
            set
            {
                this._selectedJournalParameter = value;
                RaisePropertyChanged();

            }
        }
    }
}
