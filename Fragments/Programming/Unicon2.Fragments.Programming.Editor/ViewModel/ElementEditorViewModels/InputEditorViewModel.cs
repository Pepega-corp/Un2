﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InputEditorViewModel : ViewModelBase, IInputEditorViewModel
    {
        private const string BASE_NAMING = "Base";
        private IInputEditor _model;
        private List<Dictionary<int, string>> _allInputSignals;
        private BindableKeyValuePair<int, string> _selectedInputSignal;
        private EditableListItem _selectedBase;

        public string ElementName => "Вход";
        public string Symbol => "In";
        public string Description =>"Входной логический сигнал";
        public string StrongName => ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public bool IsEditable => true;

        public InputEditorViewModel(ILibraryElement model)
        {
            this.Bases = new ObservableCollection<EditableListItem>();
            this.InputSignals = new ObservableCollection<BindableKeyValuePair<int, string>>();          
            this._allInputSignals = new List<Dictionary<int, string>>();

            this.AddBaseCommand = new RelayCommand(this.AddBase);
            this.RemoveBaseCommand = new RelayCommand(this.RemoveSelectedBase, this.CanRemoveBase);
            this.AddSignalCommand = new RelayCommand(this.AddInputSignal, this.CanAddInputSignal);
            this.RemoveSignalCommand = new RelayCommand(this.RemoveInputSignal, this.CanRemoveInputSignal);
            
            SetModel(model);
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


        public ObservableCollection<EditableListItem> Bases { get; }

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
        
        public EditableListItem SelectedBaseItem
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

        private IInputEditor GetModel()
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
            if (!(value is IInputEditor model))
                return;

            if (_model == null)
            {
                model.InitializeDefault();
                _model = model;
            }
            else
            {
                _model.AllInputSignals.Clear();
                _model.AllInputSignals.AddRange(model.AllInputSignals);
                _model.Bases.Clear();
                _model.Bases.AddRange(model.Bases);
            }

            _allInputSignals.Clear();
            foreach (var inputSignal in this._model.AllInputSignals)
            {
                this._allInputSignals.Add(new Dictionary<int, string>(inputSignal));
            }
                        
            var editableListBases = this._model.Bases.Select(@base => new EditableListItem(@base)).ToList();
            Bases.Clear();
            this.Bases.AddCollection(editableListBases);
        }

        private void AddBase()
        {
            string newBaseStr = BASE_NAMING + this.Bases.Count;
            this.Bases.Add(new EditableListItem(newBaseStr));
            
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
            return new InputEditorViewModel(new InputEditor());
        }
    }
}
