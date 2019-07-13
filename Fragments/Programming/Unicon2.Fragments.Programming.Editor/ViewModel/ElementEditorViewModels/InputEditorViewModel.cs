using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InputEditorViewModel : ViewModelBase, IInputEditorViewModel
    {
        private IInput _model;
        private Dictionary<int, Dictionary<int, string>> _allInputSignals;
        private int _selectedBase;

        public string ElementName { get { return "Вход"; } }

       public string Description { get { return "Входной логический сигнал"; } }

        public string StrongName
        {
            get { return ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL; }
        }

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void BasesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this._allInputSignals = this._allInputSignals ?? new Dictionary<int, Dictionary<int, string>>();
                    if (!this._allInputSignals.ContainsKey(this.Bases.Count - 1))
                    {
                        this._allInputSignals.Add(this.Bases.Count - 1, new Dictionary<int, string>());
                        this._allInputSignals[this.Bases.Count - 1].Add(0, string.Empty);
                    }
                    this.SelectedBase = this.Bases.Count - 1;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this._allInputSignals.Remove(this.SelectedBase);
                    this.SelectedBase = this.Bases.Count - 1;
                    break;
            }
        }

        public ObservableCollection<string> Bases { get; private set; }
        public bool NeedBases { get; set; }
        public Dictionary<int, string> InputSignals { get; private set; }

        public int SelectedBase
        {
            get { return this._selectedBase; }
            set
            {
                if(this._selectedBase == value)
                    return;

                this._selectedBase = value;
                this.InputSignals = this._allInputSignals[this._selectedBase];

                OnPropertyChanged();
                OnPropertyChanged(nameof(this.InputSignals));
            }
        }

        private IInput GetModel()
        {
            if (this._model != null)
            {
                this._model.Bases.Clear();
                this._model.Bases.AddRange(this.Bases);

                this._model.AllInputSignals.Clear();
                foreach (var inputSignal in this._allInputSignals)
                {
                    this._model.AllInputSignals.Add(inputSignal.Key, new Dictionary<int, string>(inputSignal.Value));
                }
            }

            return this._model;
        }

        private void SetModel(object value)
        {
            if(value == null) 
                return;

            if (this._model != null)
            {
                this.Bases.CollectionChanged -= this.BasesOnCollectionChanged;
                this.Bases.Clear();
                this._allInputSignals.Clear();
            }

            this._model = (IInput) value;

            this._allInputSignals = new Dictionary<int, Dictionary<int, string>>();
            foreach (var inputSignal in this._model.AllInputSignals)
            {
                this._allInputSignals.Add(inputSignal.Key, new Dictionary<int, string>(inputSignal.Value));
            }

            this.Bases = new ObservableCollection<string>();
            this.Bases.CollectionChanged += this.BasesOnCollectionChanged;
            this.Bases.AddCollection(this._model.Bases);
        }

        public object Clone()
        {
            InputEditorViewModel clone = new InputEditorViewModel();
            clone.Model = this._model.Clone();
            return clone;
        }
    }
}
