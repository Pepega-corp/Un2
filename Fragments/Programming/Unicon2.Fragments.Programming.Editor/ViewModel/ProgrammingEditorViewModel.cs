using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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
        private readonly IProgrammModelEditor _model;
        private ILogicElementEditorViewModel _selectedNewLogicElemItem;
        private ILogicElementEditorViewModel _selectedLibraryElemItem;
        private string _mrNumber;
        private string _versionHeader = "00001";
        private string _subversionHeader = "00001";

        public string StrongName => ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;

        public ICommand AddElementCommand { get; }
        public ICommand RemoveElementCommand { get; }
        public ICommand EditElementCommand { get; }

        public ProgrammingEditorViewModel(IProgrammModelEditor model, IApplicationGlobalCommands globalCommands, ILogicElementFactory logicElementFactory)
        {
            this._model = model;
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

        public bool EnableFileDriver
        {
            get => this._model.EnableFileDriver;
            set
            {
                this._model.EnableFileDriver = value;
                RaisePropertyChanged();
            }
        }

        public bool WithHeader
        {
            get => this._model.WithHeader;
            set
            {
                this._model.WithHeader = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(this.SelectedVersionHeader));
                RaisePropertyChanged(nameof(this.SelectedSubversionHeader));
            }
        }

        public string MrNumber
        {
            get => this._mrNumber;
            set
            {
                this._mrNumber = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedVersionHeader
        {
            get => this._versionHeader;
            set
            {
                this._versionHeader = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedSubversionHeader
        {
            get => this._subversionHeader;
            set
            {
                this._subversionHeader = value;
                RaisePropertyChanged();
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
            this._model.Elements.Clear();
	        this._model.Elements.AddRange(elementModels);

            if (this._model.WithHeader)
            {
                this._model.LogicHeader = $"MR{this.MrNumber} LOGIKA PROG VER. {this.SelectedVersionHeader} SUBVER. {this.SelectedSubversionHeader}";
                
                while (this._model.LogicHeader.Length < 44)
                {
                    this._model.LogicHeader += " ";
                }
            }

            return this._model;
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IProgrammModelEditor model)
            {
                this._model.EnableFileDriver = model.EnableFileDriver;
                this._model.WithHeader = model.WithHeader;
                this._model.Elements.AddRange(model.Elements);
                this.LibraryElements.Clear();
                this.LibraryElements.AddCollection(this._logicElementFactory.GetAllElementsEditorViewModels(this._model.Elements));

                if (this._model.WithHeader)
                {
                    var headers = this._model.LogicHeader.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    this.MrNumber = headers[0].Remove(0, 2);
                    this.SelectedVersionHeader = headers[4];
                    this.SelectedSubversionHeader = headers[6];
                }
                else
                {
                    this.MrNumber = string.Empty;
                    this.SelectedVersionHeader = "00001";
                    this.SelectedSubversionHeader = "00001";
                }
            }
        }
    }
}
