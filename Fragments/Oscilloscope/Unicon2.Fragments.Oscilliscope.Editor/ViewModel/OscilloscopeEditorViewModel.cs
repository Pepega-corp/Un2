using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.Editor.ViewModel
{
    public class OscilloscopeEditorViewModel : ViewModelBase, IOscilloscopeEditorViewModel
    {
        private readonly IFragmentEditorViewModelFactory _fragmentEditorViewModelFactory;
        private IOscilloscopeModel _oscilloscopeModel;
        private IOscillogramLoadingParametersEditorViewModel _oscillogramLoadingParametersEditorViewModel;
        private bool _isOscilloscopeJournalTabSelected;
        private IRecordTemplateEditorViewModel _countingTemplateEditorViewModel;
        private ICountingTemplate _countingTemplate;

        public OscilloscopeEditorViewModel(ICountingTemplate countingTemplate, IOscilloscopeModel oscilloscopeModel,
            IUniconJournalEditorViewModel uniconJournalEditorViewModel, IFragmentEditorViewModelFactory fragmentEditorViewModelFactory,
            IOscillogramLoadingParametersEditorViewModel oscillogramLoadingParametersEditorViewModel, IRecordTemplateEditorViewModel recordTemplateEditorViewModel)
        {
            this._oscilloscopeModel = oscilloscopeModel;
            this._countingTemplate = countingTemplate;
            this._fragmentEditorViewModelFactory = fragmentEditorViewModelFactory;
            this.OscilloscopeJournalEditorViewModel = uniconJournalEditorViewModel;

            this.OscillogramLoadingParametersEditorViewModel = oscillogramLoadingParametersEditorViewModel;
            this.CountingTemplateEditorViewModel = recordTemplateEditorViewModel;
        }


        public IUniconJournalEditorViewModel OscilloscopeJournalEditorViewModel { get; set; }

        public IOscillogramLoadingParametersEditorViewModel OscillogramLoadingParametersEditorViewModel
        {
            get { return this._oscillogramLoadingParametersEditorViewModel; }
            set
            {
                this._oscillogramLoadingParametersEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public IRecordTemplateEditorViewModel CountingTemplateEditorViewModel
        {
            get { return this._countingTemplateEditorViewModel; }
            set
            {
                this._countingTemplateEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsOscilloscopeJournalTabSelected
        {
            get { return this._isOscilloscopeJournalTabSelected; }
            set
            {
                this._isOscilloscopeJournalTabSelected = value;
                if (!value)
                {
                    this._oscillogramLoadingParametersEditorViewModel.SetAvailableParameters(this._oscilloscopeModel.OscilloscopeJournal.RecordTemplate.JournalParameters);
                }
            }
        }

        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private IOscilloscopeModel GetModel()
        {
            this._oscilloscopeModel.OscilloscopeJournal = this.OscilloscopeJournalEditorViewModel.Model as IUniconJournal;
            this._oscilloscopeModel.OscillogramLoadingParameters = this.OscillogramLoadingParametersEditorViewModel.Model as IOscillogramLoadingParameters;

            this._countingTemplate.RecordTemplate = this.CountingTemplateEditorViewModel.Model as IRecordTemplate;
            this._oscilloscopeModel.CountingTemplate = this._countingTemplate;

            return this._oscilloscopeModel;
        }

        private void SetModel(object oscilloscopeModel)
        {
            this._oscilloscopeModel = oscilloscopeModel as IOscilloscopeModel;
            this.OscilloscopeJournalEditorViewModel = this._fragmentEditorViewModelFactory.CreateFragmentEditorViewModel(this._oscilloscopeModel.OscilloscopeJournal) as IUniconJournalEditorViewModel;
            this.OscillogramLoadingParametersEditorViewModel.Model = this._oscilloscopeModel.OscillogramLoadingParameters;
            this.OscillogramLoadingParametersEditorViewModel.SetAvailableParameters(this._oscilloscopeModel.OscilloscopeJournal.RecordTemplate.JournalParameters);

            this.CountingTemplateEditorViewModel.Model = this._oscilloscopeModel.CountingTemplate.RecordTemplate;
        }


        public string NameForUiKey => OscilloscopeKeys.OSCILLOSCOPE;
    }
}
