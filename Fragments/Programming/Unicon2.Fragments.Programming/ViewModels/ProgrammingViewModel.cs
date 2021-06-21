using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Fragments.Programming.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
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
        private bool _isLogicStarted;

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

            this.WriteLogicCommand = new RelayCommand(this.OnWriteCommand);
            this.ReadLogicCommand = new RelayCommand(this.OnReadCommand);
            this.StopEmulationLogic = new RelayCommand(this.OnStopEmulation);
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
        public string StrongName => ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public int SelectedTabIndex { get; set; }
        public bool IsLogicStarted
        {
            get => _isLogicStarted;
            set
            {
                _isLogicStarted = value;
                foreach (var connection in ConnectionCollection)
                {
                    connection.DebugMode = _isLogicStarted;
                }
                foreach (var shemeTab in this.SchemesCollection)
                {
                    shemeTab.IsLogicStarted = value;
                }
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ISchemeTabViewModel> SchemesCollection { get; }
        public ObservableCollection<ILogicElementViewModel> ElementsLibrary { get; }
        public ObservableCollection<IConnectionViewModel> ConnectionCollection { get; }
        public ICommand NewSchemeCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand LoadProjectCommand { get; }
        public ICommand ZoomIncrementCommand { get; }
        public ICommand ZoomDecrementCommand { get; }
        public ICommand DeleteCommand { get; }
        //READ/WRITE logic commands
        public ICommand ReadLogicCommand { get; }
        public ICommand WriteLogicCommand { get; }
        public ICommand StopEmulationLogic { get; }

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

        private IConnectionViewModel GetConnectionViewModelByNumber(int connectionNumber)
        {
            return this.ConnectionCollection.FirstOrDefault(c => c.ConnectionNumber == connectionNumber);
        }

        private void CreateNewScheme()
        {
            var schemeViewModel = new NewSchemeViewModel();
            this._applicationGlobalCommands.ShowWindowModal(() => new NewSchemeView(), schemeViewModel);
            if (schemeViewModel.DialogResult == MessageDialogResult.Affirmative)
            {
                var scemeMoedel = new SchemeModel(schemeViewModel.SchemeName, schemeViewModel.SelectedSize);
                var schemeTabViewModel = new SchemeTabViewModel(scemeMoedel, this, this._factory);
                schemeTabViewModel.CloseTabEvent += this.OnCloseTab;
                schemeTabViewModel.GetConnection += this.GetConnectionViewModelByNumber;
                this.SchemesCollection.Add(schemeTabViewModel);
            }

            (this.SaveProjectCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void OnCloseTab(ISchemeTabViewModel schemeTabViewModel)
        {
            if (this.IsLogicStarted)
            {
                var messageResult =
                    MessageBox.Show("Logic emulation is working. Do you want to stop emulation and close scheme tab?",
                        "Closing Scheme", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageResult != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            schemeTabViewModel.CloseTabEvent -= this.OnCloseTab;
            schemeTabViewModel.GetConnection += this.GetConnectionViewModelByNumber;
            this.SchemesCollection.Remove(schemeTabViewModel);

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
                this._serializerService.SerializeInFile(this._programModel, sfd.FileName);
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
            if (ofd.ShowDialog() == true)
            {
                this._programModel = this._serializerService.DeserializeFromFile<IProgramModel>(ofd.FileName);
                this.UpdateCollections(this._programModel);
                (this.SaveProjectCommand as RelayCommand).RaiseCanExecuteChanged();
            }
        }

        private void DeleteSelectedElements()
        {
            var selectedTab = this.SchemesCollection[this.SelectedTabIndex];
            selectedTab.DeleteCommand.Execute(null);
        }

        private bool CanDelete()
        {
            if (this.SchemesCollection.Count == 0) return false;
            var selectedTab = this.SchemesCollection[this.SelectedTabIndex];
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

                var connectionsInScheme = new List<IConnectionViewModel>();
                var connectorsInScheme = new List<IConnectorViewModel>();

                foreach (var logicElementVM in logicElementsVM)
                {
                    connectorsInScheme.AddRange(logicElementVM.ConnectorViewModels.Where(c => c.ConnectionNumber != -1));
                }

                foreach (var number in schemeModel.ConnectionNumbers)
                {
                    if (this.ConnectionCollection.Any(c => c.ConnectionNumber == number))
                    {
                        var connection = this.ConnectionCollection.First(c => c.ConnectionNumber == number);
                        var sinkConnectors = connectorsInScheme.Where(c => c.ConnectionNumber == connection.ConnectionNumber && c.Orientation == ConnectorOrientation.LEFT).ToArray();
                        connection.SinkConnectors.AddCollection(sinkConnectors);
                        connectionsInScheme.Add(connection);
                    }
                    else
                    {
                        // get connectors with same connection number
                        var sourceConnector = connectorsInScheme.First(c => c.ConnectionNumber == number && c.Orientation == ConnectorOrientation.RIGHT);
                        var sinkConnectors = connectorsInScheme.Where(c => c.ConnectionNumber == number && c.Orientation == ConnectorOrientation.LEFT).ToArray();
                        var connectionModel = model.Connections.First(c => c.ConnectionNumber == number);
                        var connection = new ConnectionViewModel(connectionModel, sourceConnector, sinkConnectors);
                        connectionsInScheme.Add(connection);
                        this.ConnectionCollection.Add(connection);
                    }
                }

                var schemeVM = new SchemeTabViewModel(schemeModel, this, this._factory);
                schemeVM.CloseTabEvent += this.OnCloseTab;
                schemeVM.GetConnection += this.GetConnectionViewModelByNumber;
                schemeVM.ElementCollection.AddCollection(logicElementsVM);
                schemeVM.ElementCollection.AddCollection(connectionsInScheme);

                this.SchemesCollection.Add(schemeVM);
            }
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IProgramModel model)
            {
                this.ProjectName = model.ProjectName;
                this._programModel.WithHeader = model.WithHeader;
                this._programModel.EnableFileDriver = model.EnableFileDriver;
                this._programModel.LogicHeader = model.LogicHeader;
                this.UpdateCollections(this._programModel);
                this._programModel.LogBinSize = model.LogBinSize;
            }
            else if (deviceFragment is IProgrammModelEditor modelEditor)
            {
                var logicElementsViewModels = this._factory.GetAllElementsViewModels(modelEditor.Elements);
                this.ElementsLibrary.AddCollection(logicElementsViewModels);
                this._programModel.EnableFileDriver = modelEditor.EnableFileDriver;
                this._programModel.WithHeader = modelEditor.WithHeader;
                this._programModel.LogicHeader = modelEditor.LogicHeader;
                this._programModel.LogBinSize = modelEditor.LogBinSize;
            }
        }

        private async void OnWriteCommand()
        {
            if (this.SchemesCollection.Count == 0)
            {
                MessageBox.Show("Can't write logic. Scheme collection is empty!", "Write logic", MessageBoxButton.OK);
                return;
            }
            try
            {
                if (this.SchemesCollection.All(sc => sc.CanWriteToDevice))
                {
                    IsLogicStarted = true;

                    this.UpdateModelData();
                    var logicProjectBytes = this._serializerService.SerializeInBytes(this._programModel);
                    await this._logicDeviceProvider.WriteLogicArchive(logicProjectBytes, this._programModel.EnableFileDriver);

                    var logbin = this.Compile();
                    this.CalcCrc(logbin);
                    await this._logicDeviceProvider.WriteLogicProgrammBin(logbin, this._programModel.EnableFileDriver);
                    await this._logicDeviceProvider.WriteStartlogicProgrammSignal(this._programModel.EnableFileDriver);
                    
                    CycleReadingConnectionValues();

                    MessageBox.Show("Logic wrote successful!", "Write logic", MessageBoxButton.OK);
                }
                else
                {
                    throw new Exception("Not all logic elements are connected!");
                }
            }
            catch(Exception e)
            {
                IsLogicStarted = false;
                MessageBox.Show(e.Message, "Write logic", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ushort[] Compile()
        {
            var allElements = this.SchemesCollection.SelectMany(s => s.ElementCollection.Where(e => e is ILogicElementViewModel)).Cast<ILogicElementViewModel>().ToList();
            foreach (var element in allElements)
            {
                element.CompilePriority = -1;
            }
            var inputs = allElements.Where(e => e.ElementType == ElementType.In);
            foreach (var input in inputs)
            {
                input.CompilePriority = 0;
                this.SortElementsByPriority(input);
            }

            var binFile = new List<ushort>();

            if (this._programModel.WithHeader)
                binFile.AddRange(this.GetHeaderBin());

            binFile.AddRange(this.GetSchemeBin(allElements));
            binFile.AddRange(this.GetEndFileBin());

            if (binFile.Count < this._programModel.LogBinSize)
            {
                var filler = new ushort[this._programModel.LogBinSize - binFile.Count];
                binFile.AddRange(filler);
            }
            else if(binFile.Count > this._programModel.LogBinSize)
            {
                throw new Exception($"Logic programm bin has size more than {this._programModel.LogBinSize} bytes!");
            }

            return binFile.ToArray();
        }

        private void SortElementsByPriority(ILogicElementViewModel element)
        {
            var priority = element.CompilePriority + 1;

            foreach (var connector in element.ConnectorViewModels.Where(c => c.Orientation == ConnectorOrientation.RIGHT))
            {
                var connection = connector.Connection;
                foreach (var sinkConnector in connection.SinkConnectors)
                {
                    var connectedElement = sinkConnector.ParentViewModel;
                    if (connectedElement.CompilePriority == -1 || connectedElement.CompilePriority > priority)
                    {
                        connectedElement.CompilePriority = priority;
                    }

                    this.SortElementsByPriority(connectedElement);
                }
            }
        }

        private ushort[] GetHeaderBin()
        {
            var aouthdrByte = Encoding.ASCII.GetBytes(this._programModel.LogicHeader);
            var bindata = new ushort[10 + aouthdrByte.Length / 2];
            bindata[0] = 1234;// Магическое число
            bindata[1] = 0;// Количество секций
            bindata[2] = 0;// Время и дата создания
            bindata[3] = 0;// Время и дата создания
            bindata[4] = 0;//* Указатель в файле на таблицу имен
            bindata[5] = 0;//* Указатель в файле на таблицу имен
            bindata[6] = 0;//* Число элементов в таблице имен
            bindata[7] = 0;//* Число элементов в таблице имен
            bindata[8] = (ushort)aouthdrByte.Length;//* Размер вспомогательного заголовка
            bindata[9] = 0;//* Флаги
            for (var i = 0; i < aouthdrByte.Length / 2; i++)
            {
                ushort data = aouthdrByte[2 * i + 1];
                bindata[10 + i] = (ushort)((ushort)(data << 8) + aouthdrByte[2 * i]);
            }

            return bindata;
        }

        private ushort[] GetSchemeBin(List<ILogicElementViewModel> allElements)
        {
            var maxPriority = allElements.Max(e => e.CompilePriority);
            var retBin = new List<ushort>();
            for (var priority = 0; priority <= maxPriority; priority++)
            {
                foreach (var element in allElements)
                {
                    if (element.CompilePriority == priority)
                    {
                        retBin.AddRange(element.Model.GetProgramBin());
                    }
                }
            }

            return retBin.ToArray();
        }

        private ushort[] GetEndFileBin()
        {
            var bindata = new ushort[1];
            bindata[0] = 0x0001;
            return bindata;
        }

        private void CalcCrc(ushort[] binFile)
        {
            byte[] tbuff = binFile.UshortArrayToByteArray(false);
            ushort crc = CRC16.CalcCrcFast(tbuff, tbuff.Length - 2);
            binFile[binFile.Length - 1] = (ushort)(
                (ushort)((ushort)(crc >> (ushort)8) & (ushort)0x00ff) +
                (ushort)((ushort)(crc << (ushort)8) & (ushort)0xff00));
        }

        private void OnStopEmulation()
        {
            IsLogicStarted = false;
        }

        private async void CycleReadingConnectionValues()
        {
            while (IsLogicStarted)
            {
                var values = await _logicDeviceProvider.ReadConnectionValues(ConnectionCollection.Count, this._programModel.EnableFileDriver);
                for(var i = 0; i < ConnectionCollection.Count; i++)
                {
                    ConnectionCollection[i].CurrentValue = values[i];
                }
            }
        }

        private async void OnReadCommand()
        {
            if (this.SchemesCollection.Count > 0)
            {
                var result = MessageBox.Show("Would you like to save logic project?", "Write logic", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Cancel)
                    return;

                if (result == MessageBoxResult.Yes)
                {
                    this.SaveProject();
                }
            }
            try
            {
                var logicProjectBytes = await this._logicDeviceProvider.ReadLogicArchive(this._programModel.EnableFileDriver);
                var programModel = this._serializerService.DeserializeFromBytes<IProgramModel>(logicProjectBytes);
                Initialize(programModel);
                UpdateCollections(programModel);
                this.UpdateModelData();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Read logic", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
