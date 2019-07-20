using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.Factories;
using Unicon2.Fragments.Oscilliscope.Editor.Interfaces.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Oscilliscope.Editor.ViewModel.OscillogramLoadingParameters
{
    public class OscillogramLoadingParametersEditorViewModel : ViewModelBase, IOscillogramLoadingParametersEditorViewModel
    {
        private readonly IOscilloscopeTagEditorViewModelFactory _oscilloscopeTagEditorViewModelFactory;
        private bool _isFullPageLoading;
        private ushort _addressOfOscillogram;
        private IOscillogramLoadingParameters _oscillogramLoadingParameters;
        private List<IJournalParameter> _journalParameters;
        private List<string> _tagList;
        private ushort _maxSizeOfRewritableOscillogramInMs;


        public OscillogramLoadingParametersEditorViewModel(IOscilloscopeTagEditorViewModelFactory oscilloscopeTagEditorViewModelFactory, IOscillogramLoadingParameters oscillogramLoadingParameters)
        {
            this._oscillogramLoadingParameters = oscillogramLoadingParameters;
            this._oscilloscopeTagEditorViewModelFactory = oscilloscopeTagEditorViewModelFactory;
            this.OscilloscopeTagEditorViewModels = new ObservableCollection<IOscilloscopeTagEditorViewModel>();

            this.AddTagCommand = new RelayCommand(this.OnAddTagExecute);
            this.DeleteTagCommand = new RelayCommand<object>(this.OnDeleteTagExecute);

            this._tagList = new List<string>
            {
                OscilloscopeKeys.BEGIN,
                OscilloscopeKeys.AFTER,
                OscilloscopeKeys.ALM,
                OscilloscopeKeys.LEN,
                OscilloscopeKeys.POINT,
                OscilloscopeKeys.REZ,
                OscilloscopeKeys.DATATIME,
                OscilloscopeKeys.READY
            };
        }

        private void OnDeleteTagExecute(object obj)
        {
            if (obj is IOscilloscopeTagEditorViewModel)
            {
                this.OscilloscopeTagEditorViewModels.Remove((obj as IOscilloscopeTagEditorViewModel));
            }

        }

        private void OnAddTagExecute()
        {
            IOscilloscopeTagEditorViewModel oscilloscopeTagEditorViewModel =
                this._oscilloscopeTagEditorViewModelFactory.CreateOscilloscopeTagEditorViewModel();
            oscilloscopeTagEditorViewModel.SetAvailableOptions(this._journalParameters, this._tagList);
            this.OscilloscopeTagEditorViewModels.Add(oscilloscopeTagEditorViewModel);
        }


        #region Implementation of IOscillogramLoadingParametersEditorViewModel

        public ObservableCollection<IOscilloscopeTagEditorViewModel> OscilloscopeTagEditorViewModels { get; }

        public ushort AddressOfOscillogram
        {
            get { return this._addressOfOscillogram; }
            set
            {
                this._addressOfOscillogram = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort MaxSizeOfRewritableOscillogramInMs
        {
            get { return this._maxSizeOfRewritableOscillogramInMs; }
            set
            {
                this._maxSizeOfRewritableOscillogramInMs = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsFullPageLoading
        {
            get { return this._isFullPageLoading; }
            set
            {
                this._isFullPageLoading = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand AddTagCommand { get; }
        public ICommand DeleteTagCommand { get; }
        public void SetAvailableParameters(List<IJournalParameter> journalParameters)
        {
            this._journalParameters = journalParameters;
            foreach (IOscilloscopeTagEditorViewModel oscilloscopeTagEditorViewModel in this.OscilloscopeTagEditorViewModels)
            {
                oscilloscopeTagEditorViewModel.SetAvailableOptions(this._journalParameters, this._tagList);
            }
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => nameof(OscillogramLoadingParametersEditorViewModel);

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {
                this._oscillogramLoadingParameters.OscilloscopeTags.Clear();
                foreach (IOscilloscopeTagEditorViewModel oscilloscopeTagEditorViewModel in this.OscilloscopeTagEditorViewModels)
                {
                    if (oscilloscopeTagEditorViewModel.SelectedTag != null && oscilloscopeTagEditorViewModel.SelectedJournalParameter != null)
                        this._oscillogramLoadingParameters.OscilloscopeTags.Add(oscilloscopeTagEditorViewModel.Model as IOscilloscopeTag);
                }
                this._oscillogramLoadingParameters.AddressOfOscillogram = this.AddressOfOscillogram;
                this._oscillogramLoadingParameters.IsFullPageLoading = this.IsFullPageLoading;
                this._oscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs = this.MaxSizeOfRewritableOscillogramInMs;
                return this._oscillogramLoadingParameters;

            }
            set
            {
                if (value == null) return;
                this._oscillogramLoadingParameters = value as IOscillogramLoadingParameters;

                this.OscilloscopeTagEditorViewModels.Clear();
                foreach (IOscilloscopeTag oscilloscopeTag in this._oscillogramLoadingParameters.OscilloscopeTags)
                {
                    IOscilloscopeTagEditorViewModel oscilloscopeTagEditorViewModel = this._oscilloscopeTagEditorViewModelFactory.CreateOscilloscopeTagEditorViewModel(oscilloscopeTag);
                    this.OscilloscopeTagEditorViewModels.Add(oscilloscopeTagEditorViewModel);
                }
                this.AddressOfOscillogram = this._oscillogramLoadingParameters.AddressOfOscillogram;
                this.IsFullPageLoading = this._oscillogramLoadingParameters.IsFullPageLoading;
                this.MaxSizeOfRewritableOscillogramInMs = this._oscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs;
            }
        }

        #endregion
    }
}
