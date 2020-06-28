using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Editor.Interfaces;
using Unicon2.Fragments.Programming.Editor.Models;
using Unicon2.Fragments.Programming.Editor.View;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel
{
    public class ProgrammingEditorViewModel : ViewModelBase, IProgrammingEditorViewModel
    {
        private readonly ILogicElementFactory _logicElementFactory;
        private readonly IApplicationGlobalCommands _globalCommands;
        private IProgrammModelEditor _model;
        private ILogicElementEditorViewModel _selectedNewLogicElemItem;
        private ILogicElementEditorViewModel _selectedLibraryElemItem;

        public string StrongName => ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;

        public ICommand AddElementCommand { get; }
        public ICommand RemoveElementCommand { get; }
        public ICommand EditElementCommand { get; }

        public ProgrammingEditorViewModel(IApplicationGlobalCommands globalCommands, ILogicElementFactory logicElementFactory)
        {
            this._globalCommands = globalCommands;
            this._logicElementFactory = logicElementFactory;
            this.BooleanElements = new ObservableCollection<ILogicElementEditorViewModel>(this._logicElementFactory.GetBooleanElementsEditorViewModels());
            this.AnalogElements = new ObservableCollection<ILogicElementEditorViewModel>(this._logicElementFactory.GetAnalogElementsEditorViewModels());
            this.LibraryElements = new ObservableCollection<ILogicElementEditorViewModel>();

            this.AddElementCommand = new RelayCommand(this.OnAddElement);
            this.RemoveElementCommand = new RelayCommand(this.OnRemoveElement);
            this.EditElementCommand = new RelayCommand<object>(this.OnEditElement);
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

                ((RelayCommand<object>)this.EditElementCommand).RaiseCanExecuteChanged();
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

        private void OnEditElement(object element)
        {
            if (element is ILogicElementEditorViewModel logicElementEditorViewModel)
            {
                this._globalCommands.ShowWindowModal(() => new EditElementView(),
                    new EditElementViewModel(logicElementEditorViewModel));
            }
        }
        
        public IDeviceFragment BuildDeviceFragment()
        {
	        var elementModels = this.LibraryElements.Select(l => l.Model).Cast<ILibraryElement>().ToArray();
            this._model = _model ?? new ProgrammModelEditor();
            this._model.Elements.Clear();
	        this._model.Elements.AddRange(elementModels);
	        return this._model;
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IProgrammModelEditor model)
            {
                _model = model;
                this.LibraryElements.Clear();
                this.LibraryElements.AddCollection(this._logicElementFactory.GetAllElementsEditorViewModels(this._model.Elements));
            }
        }
    }
}
