using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InputEditorViewModel : ViewModelBase, IInputEditorViewModel
    {
        private IInput _model;
        private List<Dictionary<int, string>> _allInputSignals;
        private BindableKeyValuePair<int, string> _selectedInputSignal;
        private EditableBase _selectedBase;

        public string ElementName { get { return "Вход"; } }

       public string Description { get { return "Входной логический сигнал"; } }

        public string StrongName
        {
            get { return ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL; }
        }

        public InputEditorViewModel()
        {
            this.Bases = new ObservableCollection<EditableBase>();
            this.InputSignals = new ObservableCollection<BindableKeyValuePair<int, string>>();          
            this._allInputSignals = new List<Dictionary<int, string>>();

            this.AddBaseCommand = new RelayCommand(this.AddBase);
            this.RemoveBaseCommand = new RelayCommand(this.RemoveSelectedBase, this.CanRemoveBase);
            this.AddSignalCommand = new RelayCommand(this.AddInputSignal, this.CanAddInputSignal);
            this.RemoveSignalCommand = new RelayCommand(this.RemoveInputSignal, this.CanRemoveInputSignal);
        }

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        public ICommand AddBaseCommand { get; }
        public ICommand RemoveBaseCommand { get; }
        public ICommand AddSignalCommand { get; }
        public ICommand RemoveSignalCommand { get; }


        public ObservableCollection<EditableBase> Bases { get; }

        public ObservableCollection<BindableKeyValuePair<int, string>> InputSignals { get; }

        public BindableKeyValuePair<int, string> SelectedInputSignal
        {
            get => this._selectedInputSignal;
            set
            {
                if (this._selectedInputSignal == value)
                    return;

                if (this._selectedInputSignal != null)
                {
                    this._selectedInputSignal.IsInEditMode = false;
                }

                this._selectedInputSignal = value;

                if (this._selectedInputSignal != null)
                {
                    this._selectedInputSignal.IsInEditMode = true;
                }

                (this.RemoveSignalCommand as RelayCommand)?.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
        
        public EditableBase SelectedBaseItem
        {
            get => this._selectedBase;
            set
            {
                if(this._selectedBase == value)
                    return;

                if (this._selectedBase != null && this.Bases.Contains(this._selectedBase))
                {
                    this._selectedBase.IsEditing = false;
                    var index = this.Bases.IndexOf(this._selectedBase);
                    this._allInputSignals[index] = this.InputSignals.ToDictionary(inpSign => inpSign.Key, inpSign => inpSign.Value);
                }

                this._selectedBase = value;
                this.InputSignals.Clear();

                if (this._selectedBase != null)
                {
                    var index = this.Bases.IndexOf(this._selectedBase);
                    this.Bases[index].IsEditing = true;

                    foreach (KeyValuePair<int, string> kvp in this._allInputSignals[index])
                    {
                        this.InputSignals.Add(new BindableKeyValuePair<int, string>(kvp.Key, kvp.Value));
                    }
                    RaisePropertyChanged(nameof(this.InputSignals));
                }

                (this.RemoveBaseCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.AddSignalCommand as RelayCommand)?.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private IInput GetModel()
        {
            if (this._model != null)
            {
                this._model.Bases.Clear();
                this._model.Bases.AddRange(this.Bases.Select(b=>b.Value));
                //Commit edit InputSignals
                if (this._selectedInputSignal != null)
                {
                    this._selectedInputSignal.IsInEditMode = false;
                }
                //Copy InputSignals to AllInputSignals
                if (this._selectedBase != null)
                {
                    var index = this.Bases.IndexOf(this._selectedBase);
                    this._allInputSignals[index] =
                        this.InputSignals.ToDictionary(inpSign => inpSign.Key, inpSign => inpSign.Value);
                }

                this._model.AllInputSignals.Clear();
                foreach (var inputSignal in this._allInputSignals)
                {
                    this._model.AllInputSignals.Add(new Dictionary<int, string>(inputSignal));
                }
            }

            return this._model;
        }

        private void SetModel(object value)
        {
            var model = value as IInput;
            if(model == null) 
                return;

            if (this._model != null)
            {
                this.Bases.Clear();
                this._allInputSignals.Clear();
            }

            this._model = model;
            
            foreach (var inputSignal in this._model.AllInputSignals)
            {
                this._allInputSignals.Add(new Dictionary<int, string>(inputSignal));
            }

            List<EditableBase> bases = this._model.Bases.Select(@base => new EditableBase(@base)).ToList();
            this.Bases.AddCollection(bases);
        }

        private void AddBase()
        {
            this.Bases.Add(new EditableBase(this.Bases.Count));
            
            this._allInputSignals.Add(new Dictionary<int, string>());
            this._allInputSignals[this.Bases.Count - 1].Add(0, string.Empty);
            
            this.SelectedBaseItem = this.Bases.Last();
        }

        private void RemoveSelectedBase()
        {
            if (this.Bases.Contains(this.SelectedBaseItem))
            {
                var index = this.Bases.IndexOf(this.SelectedBaseItem);
                this.Bases.Remove(this.SelectedBaseItem);
                this._allInputSignals.RemoveAt(index);
            }
        }

        private bool CanRemoveBase()
        {
            return this.Bases.Count > 1 && this.SelectedBaseItem != null;
        }

        private void AddInputSignal()
        {
            var index = this.InputSignals.Last().Key;
            this.InputSignals.Add(new BindableKeyValuePair<int, string>(index+1, string.Empty));
        }

        private bool CanAddInputSignal()
        {
            return this.SelectedBaseItem != null;
        }

        private void RemoveInputSignal()
        {
            if (this.InputSignals.Contains(this.SelectedInputSignal))
            {
                this.InputSignals.Remove(this.SelectedInputSignal);
            }
        }

        private bool CanRemoveInputSignal()
        {
            return this.InputSignals.Count > 1 && this.SelectedInputSignal != null;
        }

        public object Clone()
        {
            InputEditorViewModel clone = new InputEditorViewModel();
            clone.Model = this._model.Clone();
            return clone;
        }
    }
}
