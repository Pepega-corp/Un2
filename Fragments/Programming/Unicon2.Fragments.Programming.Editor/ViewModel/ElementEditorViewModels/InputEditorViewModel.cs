using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InputEditorViewModel : ViewModelBase, IInputEditorViewModel
    {
        private IInput _model;
        private Dictionary<int, Dictionary<int, string>> _allInputSignals;
        private BindableKeyValuePair<int, string> _selectedInputSignal;
        private int _selectedBase;

        public string ElementName { get { return "Вход"; } }

       public string Description { get { return "Входной логический сигнал"; } }

        public string StrongName
        {
            get { return ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL; }
        }

        public InputEditorViewModel()
        {
            this.Bases = new ObservableCollection<string>();
            this._selectedBase = -1;

            this.InputSignals = new ObservableCollection<BindableKeyValuePair<int, string>>();          

            this._allInputSignals = new Dictionary<int, Dictionary<int, string>>();
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


        public ObservableCollection<string> Bases { get; }

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

                RaisePropertyChanged();
            }
        }

        public int SelectedBase
        {
            get { return this._selectedBase; }
            set
            {
                if(this._selectedBase == value)
                    return;

                if (this._selectedBase != -1)
                {
                    this._allInputSignals[this._selectedBase] =
                        this.InputSignals.ToDictionary(inpSign => inpSign.Key, inpSign => inpSign.Value);
                }
           
                this._selectedBase = value;

                this.InputSignals.Clear();
                foreach (KeyValuePair<int, string> kvp in this._allInputSignals[this._selectedBase])
                {
                    this.InputSignals.Add(new BindableKeyValuePair<int, string>(kvp.Key, kvp.Value));
                }

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
                //Commit edit InputSignals
                if (this._selectedInputSignal != null)
                {
                    this._selectedInputSignal.IsInEditMode = false;
                }
                //Copy InputSignals to AllInputSignals
                if (this._selectedBase != -1)
                {
                    this._allInputSignals[this._selectedBase] =
                        this.InputSignals.ToDictionary(inpSign => inpSign.Key, inpSign => inpSign.Value);
                }

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
            
            foreach (var inputSignal in this._model.AllInputSignals)
            {
                this._allInputSignals.Add(inputSignal.Key, new Dictionary<int, string>(inputSignal.Value));
            }
            
            this.Bases.CollectionChanged += this.BasesOnCollectionChanged;
            this.Bases.AddCollection(this._model.Bases);
        }

        private void BasesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
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

        //public void CopyValues(ILogicElementEditorViewModel other)
        //{
        //    InputEditorViewModel copying = other as InputEditorViewModel;
        //    if (copying == null)
        //    {
        //        throw new ArgumentException("Argument is not InputEditorViewModel");
        //    }

        //    this.Bases.Clear();
        //    this.Bases.AddCollection(copying.Bases);

        //    this._allInputSignals.Clear();
        //    foreach (var inputSignal in copying._allInputSignals)
        //    {
        //        this._allInputSignals.Add(inputSignal.Key, new Dictionary<int, string>(inputSignal.Value));
        //    }
        //}

        public object Clone()
        {
            InputEditorViewModel clone = new InputEditorViewModel();
            clone.Model = this._model.Clone();
            return clone;
        }
    }
}
