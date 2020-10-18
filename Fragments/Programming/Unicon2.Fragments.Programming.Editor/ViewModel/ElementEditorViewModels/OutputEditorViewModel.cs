using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class OutputEditorViewModel : ViewModelBase, IOutputEditorViewModel
    {
        private IOutputEditor _model;
        private EditableListItem _selectedOutputSignal;

        public string ElementName => "Выход";
        public string Description => "Выходной логический сигнал";

        public string StrongName => ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        public bool IsEditable => true;

        public ICommand AddOutputSignalCommand { get; }
        public ICommand RemoveOutputSignalCommand { get; }

        public OutputEditorViewModel(IOutputEditor model)
        {
            this.OutputSignals = new ObservableCollection<EditableListItem>();
            this.OutputSignals.CollectionChanged += this.OutputSignalsOnCollectionChanged;

            this.AddOutputSignalCommand = new RelayCommand(this.OnAddOutputSignal);
            this.RemoveOutputSignalCommand = new RelayCommand(this.OnRemoveOutputSignal, this.CanRemoveOutputSignal);

            model.InitializeDefault();
            SetModel(model);
        }

        private void OutputSignalsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs evArgs)
        {
            (this.RemoveOutputSignalCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        public ObservableCollection<EditableListItem> OutputSignals { get; }

        public EditableListItem SelectedOutputSignal
        {
            get => this._selectedOutputSignal;
            set
            {
                if (this._selectedOutputSignal != null)
                {
                    this._selectedOutputSignal.IsEditing = false;
                }
                this._selectedOutputSignal = value;
                if (this._selectedOutputSignal != null)
                {
                    this._selectedOutputSignal.IsEditing = true;
                }

                RaisePropertyChanged();
                
                (this.RemoveOutputSignalCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void SetModel(object value)
        {
            if (!(value is IOutputEditor model))
            {
                return;
            }
            if(_model == null)
            {
                this._model = model;
            }
            else
            {
                _model.OutputSignals.Clear();
                _model.OutputSignals.AddRange(model.OutputSignals);
            }

            this.OutputSignals.Clear();
            var newValues = new EditableListItem[this._model.OutputSignals.Count];
            for (int i = 0; i < this._model.OutputSignals.Count; i++)
            {
                newValues[i] = new EditableListItem(this._model.OutputSignals[i]);
            }
            this.OutputSignals.AddCollection(newValues);
        }

        private object GetModel()
        {
            if (this._model == null)
                return null;

            if (this._selectedOutputSignal != null)
            {
                this._selectedOutputSignal.IsEditing = false;
            }

            this._model.OutputSignals.Clear();
            var outputSignals = this.OutputSignals.Select(os => os.Value).ToArray();
            this._model.OutputSignals.AddRange(outputSignals);

            return this._model;
        }

        private void OnAddOutputSignal()
        {
            this.OutputSignals.Add(new EditableListItem(string.Empty));
        }

        private void OnRemoveOutputSignal()
        {
            if (this.OutputSignals.Contains(this.SelectedOutputSignal))
            {
                this.OutputSignals.Remove(this.SelectedOutputSignal);
            }
        }

        private bool CanRemoveOutputSignal()
        {
            return this.OutputSignals.Count > 1 && this.SelectedOutputSignal != null;
        }

        public object Clone()
        {
            return new OutputEditorViewModel(new OutputEditor());
        }
    }
}
