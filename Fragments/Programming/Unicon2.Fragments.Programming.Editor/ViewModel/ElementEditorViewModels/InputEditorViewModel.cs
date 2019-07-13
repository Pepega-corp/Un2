using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InputEditorViewModel : ViewModelBase, IInputEditorViewModel
    {
        private IInput _model;
        private Dictionary<int, Dictionary<int, string>> _allInputSignals;
        private int _selectedBase;

        public string ElementName { get { return "Вход"; } }

       public string Description { get { return "Входной логичексий сигнал"; } }

        public string StrongName
        {
            get { return ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL; }
        }

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        public InputEditorViewModel()
        {
            this.Bases = new ObservableCollection<string>();
            this.Bases.CollectionChanged += this.BasesOnCollectionChanged;
            this.Bases.Add("Base1");
        }

        private void BasesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this._allInputSignals = this._allInputSignals ?? new Dictionary<int, Dictionary<int, string>>();
                    this._allInputSignals.Add(this.Bases.Count-1, new Dictionary<int, string>());
                    this._allInputSignals[this.Bases.Count - 1].Add(0, string.Empty);
                    this.SelectedBase = this.Bases.Count - 1;
                    return;
                case NotifyCollectionChangedAction.Remove:
                    this._allInputSignals.Remove(this.SelectedBase);
                    this.SelectedBase = this.Bases.Count - 1;
                    break;
            }
        }

        public ObservableCollection<string> Bases { get; }
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
            return this._model;
        }

        private void SetModel(object value)
        {
            this._model = (IInput) value;
        }

        public object Clone()
        {
            InputEditorViewModel clone = new InputEditorViewModel();
            clone.Model = this._model.Clone();
            return clone;
        }
    }
}
