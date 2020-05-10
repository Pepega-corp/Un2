using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Views;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ProgrammingViewModel : ViewModelBase, IProgrammingViewModel
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ILogicElementFactory _factory;
        private IProgramModel _programModel;
        
        public ProgrammingViewModel(IProgramModel model, IApplicationGlobalCommands globalCommands, ILogicElementFactory factory)
        {
            this._programModel = model;
            this._applicationGlobalCommands = globalCommands;
            this._factory = factory;
            
            this.SchemesCollection = new ObservableCollection<ISchemeTabViewModel>();
            this.ElementsLibrary = new ObservableCollection<ILogicElementViewModel>();
            this.ConnectionCollection = new ObservableCollection<IConnectionViewModel>();

            this.NewSchemeCommand = new RelayCommand(this.CreateNewScheme);
            this.SaveProjectCommand = new RelayCommand(this.SaveProject, CanSaveProject);
            this.LoadProjectCommand = new RelayCommand(this.LoadProject);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
            this.ZoomIncrementCommand = new RelayCommand(this.ZoomIncrement, this.CanZooming);
            this.ZoomDecrementCommand = new RelayCommand(this.ZoomDecrement, this.CanZooming);
        }

        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        public string ProjectName
        {
            get => this._programModel.ProjectName;
            set
            {
                if (string.Equals(this._programModel.ProjectName, value, System.StringComparison.InvariantCultureIgnoreCase))
                    return;
                this._programModel.ProjectName = value;
                RaisePropertyChanged();
            }
        }
        public string StrongName => ProgrammingKeys.PROGRAMMING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        public int SelectedTabIndex { get; set; }

        public ObservableCollection<ISchemeTabViewModel> SchemesCollection { get; }
        public ObservableCollection<ILogicElementViewModel> ElementsLibrary { get; }
        public ObservableCollection<IConnectionViewModel> ConnectionCollection { get; }

        public ICommand NewSchemeCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand LoadProjectCommand { get; }
        public ICommand ZoomIncrementCommand { get; }
        public ICommand ZoomDecrementCommand { get; }

        public void AddConnection(IConnectionViewModel connectionViewModel)
        {
            if (this.ConnectionCollection.Contains(connectionViewModel))
                return;

            this.ConnectionCollection.Add(connectionViewModel);
        }

        public void RemoveConnection(IConnectionViewModel connectionViewModel)
        {
            if (this.ConnectionCollection.Contains(connectionViewModel))
            {
                this.ConnectionCollection.Remove(connectionViewModel);
            }
        }

        public int GetNewConnectionNumber()
        {
            var number = 0;

            while (this.ConnectionCollection.Any(c => c.ConnectionNumber == number))
            {
                number++;
            }

            return number;
        }

        private void CreateNewScheme()
        {
            var schemeViewModel = new NewSchemeViewModel();
            this._applicationGlobalCommands.ShowWindowModal(()=>new NewSchemeView(), schemeViewModel);
            if (schemeViewModel.DialogResult == MessageDialogResult.Affirmative)
            {
                SchemeModel scemeMoedel = new SchemeModel(schemeViewModel.SchemeName, schemeViewModel.SelectedSize);
                SchemeTabViewModel tabViewModel = new SchemeTabViewModel(scemeMoedel, this, this._factory);
                tabViewModel.CloseTabEvent += OnCloseTab;
                this.SchemesCollection.Add(tabViewModel);
            }

            (SaveProjectCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void OnCloseTab(ISchemeTabViewModel schemeTab)
        {
            schemeTab.CloseTabEvent -= OnCloseTab;
            SchemesCollection.Remove(schemeTab);

            (SaveProjectCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void SaveProject()
        {
            var projectPath = Path.Combine(this._programModel.ProjectPath, this._programModel.ProjectName + ProgramModel.EXTENSION);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = _programModel.ProjectPath;
            sfd.FileName = ProjectName;
            sfd.Filter = $"Logic Project file (*{ProgramModel.EXTENSION})|*{ProgramModel.EXTENSION}";
            if (sfd.ShowDialog() == true)
            {
                var model = GetModel();
                model.SerializeInFile(sfd.FileName, false);
            }
        }

        private bool CanSaveProject()
        {
            return SchemesCollection.Count > 0;
        }

        private void LoadProject()
        {
            
        }

        public ICommand DeleteCommand { get; }

        private void DeleteSelectedElements()
        {
            ISchemeTabViewModel selectedTab = this.SchemesCollection[this.SelectedTabIndex];
            selectedTab.DeleteCommand.Execute(null);
        }

        private bool CanDelete()
        {
            if (this.SchemesCollection.Count == 0) return false;
            ISchemeTabViewModel selectedTab = this.SchemesCollection[this.SelectedTabIndex];
            return selectedTab.DeleteCommand.CanExecute(null);
        }

        private bool CanZooming()
        {
            return this.SchemesCollection.Count > 0;
        }

        private void ZoomIncrement()
        {
            this.SchemesCollection[this.SelectedTabIndex].ZoomIncrementCommand.Execute(this.SchemesCollection[this.SelectedTabIndex]);
        }

        private void ZoomDecrement()
        {
            this.SchemesCollection[this.SelectedTabIndex].ZoomDecrementCommand.Execute(this.SchemesCollection[this.SelectedTabIndex]);
        }

        private void SetModel(object value)
        {
            if (value is IProgramModel model)
            {
                this._programModel.Schemes = model.Schemes;

                foreach(var sceme in this._programModel.Schemes)
                {
                    this.SchemesCollection.Add(new SchemeTabViewModel(sceme, this, this._factory));
                }

                var connectors = new List<IConnectorViewModel>();

                foreach (var schemeTabViewModel in SchemesCollection)
                {
                    foreach (var logicElementViewModel in schemeTabViewModel.ElementCollection.Cast<ILogicElementViewModel>())   
                    {
                        foreach (var connectorViewModel in logicElementViewModel.ConnectorViewModels.Where(c=>c.ConnectionNumber != -1))
                        {
                            connectors.Add(connectorViewModel);
                        }
                    }
                }

                foreach (var connection in this._programModel.Connections)
                {
                    // get connectors with same connection number

                    //this.ConnectionCollection.Add(new ConnectionViewModel());
                }
                return;
            }

            if (value is IProgrammModelEditor modelEditor)
            {
                var logicElementsViewModels = this._factory.GetAllElementsViewModels(modelEditor.Elements);
                this.ElementsLibrary.AddCollection(logicElementsViewModels);
            }
        }

        private IProgramModel GetModel()
        {
            this._programModel.Schemes = this.SchemesCollection.Select(sc => sc.Model).ToArray();
            this._programModel.Connections = ConnectionCollection.Select(cc => cc.Model).ToArray();

            return this._programModel;
        }    
    }
}
