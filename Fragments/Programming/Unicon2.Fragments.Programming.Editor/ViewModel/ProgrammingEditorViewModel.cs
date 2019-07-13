using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Editor.Interfaces;
using Unicon2.Fragments.Programming.Editor.View;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel
{
    public class ProgrammingEditorViewModel : ViewModelBase, IProgrammingEditorViewModel
    {
        private IElementLibraryModel _model;
        private ILogicElementFactory _logicElementFactory;
        private IApplicationGlobalCommands _globalCommands;

        private ILogicElementEditorViewModel _selectedNewLogicElemItem;
        private ILogicElementEditorViewModel _selectedLibraryElemItem;

        public ICommand AddElementCommand { get; }
        public ICommand RemoveElementCommand { get; }
        public ICommand EditElementCommand { get; }

        public ProgrammingEditorViewModel(IApplicationGlobalCommands globalCommands, ILogicElementFactory logicElementFactory)
        {
            this._globalCommands = globalCommands;
            this._logicElementFactory = logicElementFactory;

            this.BooleanElements = new ObservableCollection<ILogicElementEditorViewModel>(this._logicElementFactory.GetBooleanElementsViewModels());
            this.AnalogElements = new ObservableCollection<ILogicElementEditorViewModel>(this._logicElementFactory.GetAnalogElementsViewModels());
            this.LibraryElements = new ObservableCollection<ILogicElementEditorViewModel>();

            this.AddElementCommand = new RelayCommand(this.OnAddElement);
            this.RemoveElementCommand = new RelayCommand(this.OnRemoveElement);
            this.EditElementCommand = new RelayCommand(this.OnEditElement, this.CanEditElement);
        }
        
        public ObservableCollection<ILogicElementEditorViewModel> BooleanElements { get; }

        public ObservableCollection<ILogicElementEditorViewModel> AnalogElements { get; }

        public ObservableCollection<ILogicElementEditorViewModel> LibraryElements { get; }

        public ILogicElementEditorViewModel SelectedNewLogicElemItem
        {
            get { return this._selectedNewLogicElemItem; }
            set
            {
                if (this._selectedNewLogicElemItem == value) return;
                this._selectedNewLogicElemItem = value;
                this.RaisePropertyChanged();
            }
        }

        public ILogicElementEditorViewModel SelectedLibraryElemItem
        {
            get { return this._selectedLibraryElemItem; }
            set
            {
                if (this._selectedLibraryElemItem == value) return;
                this._selectedLibraryElemItem = value;
                this.RaisePropertyChanged();

                ((RelayCommand)this.EditElementCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnAddElement()
        {
            if (this.SelectedNewLogicElemItem == null || this.LibraryElements.Contains(this.SelectedNewLogicElemItem)) return;

            this.LibraryElements.Add(this.SelectedNewLogicElemItem);
        }

        private void OnRemoveElement()
        {
            if (this.SelectedLibraryElemItem == null || !this.LibraryElements.Contains(this.SelectedLibraryElemItem)) return;

            this.LibraryElements.Remove(this.SelectedLibraryElemItem);
        }

        private void OnEditElement()
        {
            this._globalCommands.ShowWindowModal(() => new EditElementView(), new EditElementViewModel());
        }

        private bool CanEditElement()
        {
            return this.SelectedLibraryElemItem != null && this.LibraryElements.Count > 0;
        }

        #region Implementation of IStronglyNamed

        public string StrongName => ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private IElementLibraryModel GetModel()
        {
            IEnumerable<ILogicElement> elementModels = this.LibraryElements.Select(l => l.Model as ILogicElement);
            this._model.Elements.AddRange(elementModels);
            return this._model;
        }

        private void SetModel(object model)
        {
            if (!(model is IElementLibraryModel)) return;

            this._model = (IElementLibraryModel) model;
            this.LibraryElements.AddCollection(this._logicElementFactory.GetAllElementsViewModels(this._model.Elements));
        }

        #endregion

        #region Implementation of IFragmentEditorViewModel

        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;

        #endregion
    }
}
