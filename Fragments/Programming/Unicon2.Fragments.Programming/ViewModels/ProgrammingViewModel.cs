using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Fragments.Programming.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.DeviceContext;
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
        private readonly ISerializerService _serializerService;
        private readonly LogicDeviceProvider _logicDeviceProvider;
        private IProgramModel _programModel;
        
        public ProgrammingViewModel(IProgramModel model, LogicDeviceProvider logicDeviceProvider, 
            ILogicElementFactory factory, IApplicationGlobalCommands globalCommands, ISerializerService serializerService)
        {
            this._programModel = model;
            this._applicationGlobalCommands = globalCommands;
            this._factory = factory;

            this._serializerService = serializerService;
            this._logicDeviceProvider = logicDeviceProvider;

            this.SchemesCollection = new ObservableCollection<ISchemeTabViewModel>();
            this.ElementsLibrary = new ObservableCollection<ILogicElementViewModel>();
            this.ConnectionCollection = new ObservableCollection<IConnectionViewModel>();

            this.NewSchemeCommand = new RelayCommand(this.CreateNewScheme);
            this.SaveProjectCommand = new RelayCommand(this.SaveProject, this.CanSaveProject);
            this.LoadProjectCommand = new RelayCommand(this.LoadProject);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
            this.ZoomIncrementCommand = new RelayCommand(this.ZoomIncrement, this.CanZooming);
            this.ZoomDecrementCommand = new RelayCommand(this.ZoomDecrement, this.CanZooming);

            this.WriteLogicCommand = new RelayCommand(OnWriteCommand);
            this.ReadLogicCommand = new RelayCommand(OnWriteCommand);
        }

        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        public DeviceContext DeviceContext
        {
            get => this._logicDeviceProvider.DeviceContext;
            set
            {
                this._logicDeviceProvider.SetDeviceContext(value);
            }
        }

        public string ProjectName
        {
            get => this._programModel.ProjectName;
            set
            {
                if (string.Equals(this._programModel.ProjectName, value, System.StringComparison.InvariantCultureIgnoreCase))
                    return;
                this._programModel.ProjectName = value;
                this.RaisePropertyChanged();
            }
        }
        public string StrongName => ProgrammingKeys.PROGRAMMING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        
        public int SelectedTabIndex { get; set; }

        public ObservableCollection<ISchemeTabViewModel> SchemesCollection { get; }
        public ObservableCollection<ILogicElementViewModel> ElementsLibrary { get; }
        public ObservableCollection<IConnectionViewModel> ConnectionCollection { get; }

        public ICommand NewSchemeCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand LoadProjectCommand { get; }
        public ICommand ZoomIncrementCommand { get; }
        public ICommand ZoomDecrementCommand { get; }
        //READ/WRITE logic commands
        public ICommand ReadLogicCommand { get; }
        public ICommand WriteLogicCommand { get; }
        public ICommand StartEmulationCommand { get; }

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
                tabViewModel.CloseTabEvent += this.OnCloseTab;
                this.SchemesCollection.Add(tabViewModel);
            }

            (this.SaveProjectCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void OnCloseTab(ISchemeTabViewModel schemeTab)
        {
            schemeTab.CloseTabEvent -= this.OnCloseTab;
            this.SchemesCollection.Remove(schemeTab);

            (this.SaveProjectCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void SaveProject()
        {
            var sfd = new SaveFileDialog();
            sfd.InitialDirectory = this._programModel.ProjectPath;
            sfd.FileName = this.ProjectName;
            sfd.Filter = $"Logic Project file (*{ProgramModel.EXTENSION})|*{ProgramModel.EXTENSION}";
            if (sfd.ShowDialog() == true)
            {
                this.UpdateModelData();
                this._serializerService.SerializeInFile(this._programModel,sfd.FileName);
            }
        }

        private bool CanSaveProject()
        {
            return this.SchemesCollection.Count > 0;
        }

        private void LoadProject()
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = this._programModel.ProjectPath;
            ofd.Filter = $"Logic Project file (*{ProgramModel.EXTENSION})|*{ProgramModel.EXTENSION}";
            if(ofd.ShowDialog() == true)
            {
                this._programModel = this._serializerService.DeserializeFromFile<IProgramModel>(ofd.FileName);
                this.UpdateCollections(this._programModel);
            }
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

        private void UpdateModelData()
        {
            this._programModel.Schemes.Clear();
            this._programModel.Schemes.AddRange(this.SchemesCollection.Select(sc => sc.Model));
            this._programModel.Connections.Clear();
            this._programModel.Connections.AddRange(this.ConnectionCollection.Select(cc => cc.Model));
        }

        private void UpdateCollections(IProgramModel model)
        {
            this.SchemesCollection.Clear();
            this.ConnectionCollection.Clear();

            foreach (var schemeModel in model.Schemes)
            {
                var logicElementsVM = this._factory.GetAllElementsViewModels(schemeModel.LogicElements);

                var connections = new List<IConnectionViewModel>();
                var connectors = new List<IConnectorViewModel>();

                foreach (var logicElementVM in logicElementsVM)
                {
                    foreach (var connectorVM in logicElementVM.ConnectorViewModels.Where(c => c.ConnectionNumber != -1))
                    {
                        connectors.Add(connectorVM);
                    }
                }

                foreach (var connection in model.Connections)
                {
                    // get connectors with same connection number
                    var source = connectors.First(c => c.ConnectionNumber == connection.ConnectionNumber && c.Orientation == ConnectorOrientation.RIGHT);
                    var sink = connectors.First(c => c.ConnectionNumber == connection.ConnectionNumber && c.Orientation == ConnectorOrientation.LEFT);
                    connections.Add(new ConnectionViewModel(connection, source, sink));
                }

                this.ConnectionCollection.AddCollection(connections);

                var schemeVM = new SchemeTabViewModel(schemeModel, this, this._factory);
                schemeVM.CloseTabEvent += this.OnCloseTab;
                schemeVM.ElementCollection.AddCollection(logicElementsVM);
                schemeVM.ElementCollection.AddCollection(connections);

                this.SchemesCollection.Add(schemeVM);
            }
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
	        if (deviceFragment is IProgramModel model)
	        {
		        this._programModel = model;

                this.UpdateCollections(this._programModel);

		        return;
	        }

	        if (deviceFragment is IProgrammModelEditor modelEditor)
	        {
		        var logicElementsViewModels = this._factory.GetAllElementsViewModels(modelEditor.Elements);
		        this.ElementsLibrary.AddCollection(logicElementsViewModels);
	        }
        }

        private void OnWriteCommand()
        {
            if(SchemesCollection.Count == 0)
            {
                MessageBox.Show("Can't write logic. Scheme collection is empty!", "Write logic", MessageBoxButton.OK);
                return;
            }

            if(SchemesCollection.All(sc => sc.CanWriteToDevice))
            {
                this.UpdateModelData();
                var logicProjectBytes = _serializerService.SerializeInBytes(_programModel);
                this._logicDeviceProvider.WriteLogic(logicProjectBytes);

                //var uncompressProject = _logicReader.UncompressProject(compressedProject);
                //var model = _serializerService.DeserializeFromBytes<IProgramModel>(uncompressProject);
            }
            else
            {
                MessageBox.Show("Not all logic elements are connected!", "Write logic", MessageBoxButton.OK);
            }
        }

        private void OnReadCommand()
        {
            if (this.SchemesCollection.Count > 0)
            {
                var result = MessageBox.Show("Would you like to save logic project?", "Write logic", MessageBoxButton.YesNoCancel);

                if(result == MessageBoxResult.Cancel)
                    return;

                if (result == MessageBoxResult.Yes)
                {
                    this.SaveProject();
                }
            }

            var logicProjectBytes = this._logicDeviceProvider.ReadLogic();
        }
    }
}
