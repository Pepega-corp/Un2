using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Filter;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class ConfigurationEditorViewModel : ViewModelBase, IConfigurationEditorViewModel, IChildPositionChangeable
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private readonly IDependenciesService _dependenciesService;
        private readonly DependencyFillHelper _dependencyFillHelper;
        private readonly BaseValuesFillHelper _baseValuesFillHelper;
        private readonly ImportPropertiesFromExcelTypeAHelper _importPropertiesFromExcelTypeAHelper;
        private ObservableCollection<IConfigurationItemViewModel> _allRows;
        private IEditorConfigurationItemViewModel _selectedRow;
        private Result<(IEditorConfigurationItemViewModel item,bool isMove)> _bufferConfigurationItem=Result<(IEditorConfigurationItemViewModel item, bool isMove)>.Create(false);
        private ushort _addressIteratorValue;
        private bool _isAdditionalSettingsOpened;
        private bool _isMultiEditMode;
        private List<IEditorConfigurationItemViewModel> _selectedRows;
        private ObservableCollection<IElementAddingCommand> _elementsAddingCommandCollectionFiltered;
        private IElementAddingCommand _selectedElementsAddingCommand;
        private IBaseValuesViewModel _baseValuesViewModel;

        public ConfigurationEditorViewModel(
            IApplicationGlobalCommands applicationGlobalCommands,
            Func<IElementAddingCommand> elementAddingCommandAddingFunc,
            IFormatterEditorFactory formatterEditorFactory, IFragmentSettingsViewModel fragmentSettingsViewModel,
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel, IDependenciesService dependenciesService,
            DependencyFillHelper dependencyFillHelper, BaseValuesFillHelper baseValuesFillHelper,
            ImportPropertiesFromExcelTypeAHelper importPropertiesFromExcelTypeAHelper
        )
        {
            _allRows = new ObservableCollection<IConfigurationItemViewModel>();
            _applicationGlobalCommands = applicationGlobalCommands;
            _formatterEditorFactory = formatterEditorFactory;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            _dependenciesService = dependenciesService;
            _dependencyFillHelper = dependencyFillHelper;
            _baseValuesFillHelper = baseValuesFillHelper;
            _importPropertiesFromExcelTypeAHelper = importPropertiesFromExcelTypeAHelper;
            FragmentSettingsViewModel = fragmentSettingsViewModel;
            RootConfigurationItemViewModels = new ObservableCollection<IConfigurationItemViewModel>();
            ElementsAddingCommandCollection = new ObservableCollection<IElementAddingCommand>();
            AddRootElementCommand = new RelayCommand(OnAddRootElement);

            AddRootGroupElementCommand = new RelayCommand(OnAddRootGroupElementExecute);
            IElementAddingCommand command = elementAddingCommandAddingFunc();
            command.Name = "AddChildElement";
            command.AddingCommand = new RelayCommand(OnAddChildElementExecute, CanExecuteAddChildElement);
            ElementsAddingCommandCollection.Add(command);
            command = elementAddingCommandAddingFunc();
            command.Name = "AddChildGroup";
            command.AddingCommand =
                new RelayCommand(OnAddChildGroupElementExecute, CanExecuteAddChildGroupElement);
            ElementsAddingCommandCollection.Add(command);



            command = elementAddingCommandAddingFunc();
            command.Name = "AddComplexProperty";
            command.AddingCommand =
                new RelayCommand(OnAddComplexPropertyExecute, CanExecuteAddChildGroupElement);
            ElementsAddingCommandCollection.Add(command);

            command = elementAddingCommandAddingFunc();
            command.Name = "AddSubProperty";
            command.AddingCommand =
                new RelayCommand(OnAddAddSubPropertyExecute, CanExecuteAddSubPropertyElement);
            ElementsAddingCommandCollection.Add(command);

            //command = elementAddingCommandAddingFunc();
            //command.Name = "AddMatrix";
            //command.AddingCommand = new RelayCommand(OnAddMatrixExecute, CanExecuteAddChildGroupElement);
            //ElementsAddingCommandCollection.Add(command);

            EditElementCommand = new RelayCommand(OnEditElementExecute, CanExecuteEditElement);
            DeleteElementCommand = new RelayCommand(OnDeleteElementExecute, CanExecuteDeleteElement);
            ShowFormatterParametersCommand =
                new RelayCommand(OnShowFormatterParametersExecute, CanExecuteShowFormatterParameters);
            SetElementDownCommand = new RelayCommand(OnSetElementDownExecute, CanExecuteSetElementDown);
            SetElementUpCommand = new RelayCommand(OnSetElementUpExecute, CanExecuteSetElementUp);
            OpenConfigurationSettingsCommand = new RelayCommand(OnOpenConfigurationSettingsExecute);
            CopyElementCommand = new RelayCommand(OnCopyElementExecute, CanExecuteCopyElement);
            CutElementCommand = new RelayCommand(OnCutElementExecute, CanExecuteCutElement);

            PasteAsChildElementCommand =
                new RelayCommand(OnPasteAsChildElementExecute, CanPasteAsChildElementElement);
            

            AddSelectedElementAsResourceCommand = new RelayCommand(OnAddSelectedElementAsResourceExecute,
                CanExecuteAddSelectedElementAsResource);
            ShowDependenciesCommand = new RelayCommand(OnShowDependenciesExecute, CanExecuteShowDependencies);
            ShowFiltersCommand = new RelayCommand(OnShowFiltersExecute, CanExecuteShowFilters);
            EditDescriptionCommand =
                new RelayCommand(OnEditDescriptionExecute, CanExecuteEditDescription);
            IncreaseAddressCommand = new RelayCommand(() => OnChangeAddress(true),
                () => SelectedRows.All(model => model is IAddressChangeable));
            DecreaseAddressCommand = new RelayCommand(() => OnChangeAddress(false),
                () => SelectedRows.All(model => model is IAddressChangeable));
            TriggerAdditionalSettingsCommand = new RelayCommand(() => { IsAdditionalSettingsOpened = true; });
            AddressIteratorValue = 1;
            OnSelectionChangedCommand = new RelayCommand<object>(OnSelectionChangedExecute);
            SelectedRows = new List<IEditorConfigurationItemViewModel>();
            OpenBasicValuesCommand = new RelayCommand(OnOpenBasicValuesExacute);
            MigrateComplexPropertiesCommand =new RelayCommand(OnMigrateComplexPropertiesExecute);
            BaseValuesViewModel = new BaseValuesViewModel();
            ImportPropertiesFromExcelTypeACommand = new RelayCommand(OnImportPropertiesFromExcelTypeAExecute);
            EditCommand = new RelayCommand(OnEditExecute);
        }

        private void OnMigrateComplexPropertiesExecute()
        {
            var res = ComplexPropertiesMigrator.GetAllComplexPropertiesInConfiguration(RootConfigurationItemViewModels
                .ToList());
            if (!res.Any())
            {
                _applicationGlobalCommands.ShowErrorMessage("Составные свойства не найдены",null);
                return;
            }

            var askUserResult = _applicationGlobalCommands.AskUserGlobal(
                $"{res.Count} cоставных свойств будет смигрировано в свойства с выбранными битами. Продолжить?",
                "Миграция");
            if (askUserResult)
            {
                var migrationResult = ComplexPropertiesMigrator.MigrateComplexProperties(res);
                if (migrationResult.IsSuccess)
                {
                    MessageBox.Show($"{res.Count} составных свойств было успешно смигрировано");
                }
            }

        }

        private void OnEditExecute()
        {
            if (SelectedRow is IEditable editable)
            {
                if (editable.IsInEditMode)
                {
                    editable.StopEditElement();
                }
                else
                {
                    editable.StartEditElement();
                }
            }
        }

        private void OnImportPropertiesFromExcelTypeAExecute()
        {
           _importPropertiesFromExcelTypeAHelper.ImportPropertiesToGroup(SelectedRow as IConfigurationGroupEditorViewModel);
        }

        private void OnOpenBasicValuesExacute()
        {
            _applicationGlobalCommands.ShowWindowModal(() => new BaseValuesWindow(), new BaseValuesWindowViewModel(BaseValuesViewModel));
        }

        private bool CanExecuteShowFilters()
        {
            return SelectedRows.Count == 1 && SelectedRow is IConfigurationGroupEditorViewModel;
        }

        private void OnShowFiltersExecute()
        {
            _applicationGlobalCommands.ShowWindowModal(()=>new FilterEditingWindow(), new FilterEditorWindowViewModel(SelectedRow as IConfigurationGroupEditorViewModel));
        }

        private void OnSelectionChangedExecute(object obj)
        {
            if (obj is ObservableCollection<object> editorConfigurationItemViewModels)
            {
                SelectedRows = editorConfigurationItemViewModels.Cast<IEditorConfigurationItemViewModel>().ToList();
                IsMultiEditMode = SelectedRows.Count > 1;
            }
            (IncreaseAddressCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DecreaseAddressCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ShowFormatterParametersCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ShowFiltersCommand as RelayCommand)?.RaiseCanExecuteChanged();

        }

        private bool CanExecuteShowDependencies()
        {
            return SelectedRow is IDependenciesViewModelContainer;
        }

        private void OnShowDependenciesExecute()
        {
            _dependenciesService.EditDependencies(SelectedRow as IDependenciesViewModelContainer,
                new DependenciesConfiguration(("ConditionResultDependency",
                    () => _dependencyFillHelper.CreateEmptyConditionResultDependencyViewModel())));
        }

        private void OnChangeAddress(bool isIncreasing)
		{ 
            SelectedRows.ForEach(model => (model as IAddressChangeable).ChangeAddress(AddressIteratorValue, isIncreasing));
        }

        /*
        private void OnAddMatrixExecute()
        {
            if (SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty =
                    (SelectedRow as IChildAddable).AddMatrix() as IEditorConfigurationItemViewModel;
                PrepareAdding();
                SelectedRow = dependentProperty;
                CompleteAdding();
            }
        }
        */

        private void OnAddAddSubPropertyExecute()
        {
            if (SelectedRow is ISubPropertyAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty =
                    (SelectedRow as ISubPropertyAddable).AddSubProperty() as IEditorConfigurationItemViewModel;
                PrepareAdding();

                SelectedRow = dependentProperty;
                CompleteAdding();
            }
        }

        private bool CanExecuteAddSubPropertyElement()
        {
            return SelectedRow is ISubPropertyAddable;
        }

        private void OnAddComplexPropertyExecute()
        {
            if (SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty =
                    (SelectedRow as IChildAddable).AddComplexProperty() as IEditorConfigurationItemViewModel;
                PrepareAdding();
                SelectedRow = dependentProperty;
                CompleteAdding();
            }
        }

        private void OnEditDescriptionExecute()
        {
            _applicationGlobalCommands.ShowWindowModal(() => new DescriptionEditingWindow(),
                new DescriptionEditingViewModel() {Item = SelectedRow});
        }

        private bool CanExecuteEditDescription()
        {
            return SelectedRow != null;
        }

        private bool CanExecuteAddSelectedElementAsResource()
        {
	        return
		        _selectedRow is IPropertyEditorViewModel && !_sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesContainsViewModel(_selectedRow);
        }

        private void OnAddSelectedElementAsResourceExecute()
        {
             _sharedResourcesGlobalViewModel.AddAsSharedResourceWithContainer(_selectedRow);
        }

        private bool CanPasteAsChildElementElement()
        {
            return SelectedRow is IAsChildPasteable && _bufferConfigurationItem.IsSuccess;
        }

        private void OnPasteAsChildElementExecute()
        {
            if (_bufferConfigurationItem.IsSuccess)
            {
                if (_bufferConfigurationItem.Item.isMove)
                {
                    var sharedResourceInfo =
                         _sharedResourcesGlobalViewModel.GetNameByResourceViewModel(_bufferConfigurationItem.Item.item);

                    var selectedRowToPaste = (SelectedRow as IAsChildPasteable);


                    IEditorConfigurationItemViewModel editorConfigurationItemViewModel =
                        _bufferConfigurationItem.Item.item;
                    var parent = editorConfigurationItemViewModel.Parent;
                    parent?.Checked?.Invoke(false);

                    (editorConfigurationItemViewModel as IDeletable).DeleteElement();


                    parent.IsCheckable = true;
                    parent?.Checked?.Invoke(true);

                    selectedRowToPaste.PasteAsChild(editorConfigurationItemViewModel);


                    PrepareAdding();
                    SelectedRow = editorConfigurationItemViewModel;
                    CompleteAdding();
                    _bufferConfigurationItem =
                        Result<(IEditorConfigurationItemViewModel item, bool isMove)>.Create(false);
                    if (sharedResourceInfo.IsSuccess)
                    {
                        _sharedResourcesGlobalViewModel.AddAsSharedResourceWithContainer(editorConfigurationItemViewModel,sharedResourceInfo.Item,false);
                    }
                }
                else
                {
                    if (SelectedRow is IAsChildPasteable)
                    {
                        IEditorConfigurationItemViewModel editorConfigurationItemViewModel =
                            _bufferConfigurationItem.Item.item.Clone() as IEditorConfigurationItemViewModel;
                        (SelectedRow as IAsChildPasteable).PasteAsChild(editorConfigurationItemViewModel);

                        PrepareAdding();
                        SelectedRow = editorConfigurationItemViewModel;
                        CompleteAdding();
                    }
                }
            }
        }

        private bool CanExecuteCopyElement()
        {
            return SelectedRow is ICloneable;
        }

        private void OnCopyElementExecute()
        {
            if (SelectedRow is ICloneable)
            {
                _bufferConfigurationItem =
                    Result<(IEditorConfigurationItemViewModel item, bool isMove)>.Create((SelectedRow, false), true);
            }
        }
        private bool CanExecuteCutElement()
        {
            return SelectedRow is IDeletable;
        }

        private void OnCutElementExecute()
        {
            _bufferConfigurationItem =
                Result<(IEditorConfigurationItemViewModel item, bool isMove)>.Create((SelectedRow, true), true);
        }

        private void OnOpenConfigurationSettingsExecute()
        {
            this._applicationGlobalCommands.ShowWindowModal(() => new ConfigurationSettingsView(), FragmentSettingsViewModel);
        }

        private bool CanExecuteAddChildGroupElement()
        {
            return (SelectedRow is IChildAddable);
        }

        private void OnAddChildGroupElementExecute()
        {
            if (SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel configurationItemViewModel =
                    (SelectedRow as IChildAddable).AddChildGroupElement() as IEditorConfigurationItemViewModel;
                PrepareAdding();

                SelectedRow = configurationItemViewModel;
                CompleteAdding();
            }
        }

        private void OnAddRootGroupElementExecute()
        {
            IEditorConfigurationItemViewModel configurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().VisitItemsGroup(null);
            AllRows.Add(configurationItemViewModel);
            RootConfigurationItemViewModels.Add(configurationItemViewModel);
            SelectedRow = configurationItemViewModel;
            CompleteAdding();
        }

        private bool CanExecuteAddChildElement()
        {
            return (SelectedRow is IChildAddable);
        }

        private void OnAddChildElementExecute()
        {
            if (SelectedRow is IChildAddable parent)
            {

                IEditorConfigurationItemViewModel configurationEditorViewModel =
                    (SelectedRow as IChildAddable).AddChildElement() as IEditorConfigurationItemViewModel;
                PrepareAdding();
                SelectedRow = configurationEditorViewModel;
                
                CompleteAdding();
            }
        }

        private void OnAddRootElement()
        {
            IEditorConfigurationItemViewModel configurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null);
            AllRows.Add(configurationItemViewModel);
            RootConfigurationItemViewModels.Add(configurationItemViewModel);
            SelectedRow = configurationItemViewModel;
            CompleteAdding();
        }

        private void PrepareAdding()
        {
            if (SelectedRow != null)
            {
                SelectedRow.IsCheckable = true;
                SelectedRow?.Checked?.Invoke(false);
                SelectedRow?.Checked?.Invoke(true);
            }
        }



		private void CompleteAdding()
        {
            OnEditElementExecute();
        }



        public IEditorConfigurationItemViewModel SelectedRow
        {
            get { return _selectedRow; }
            set
            {
	            if (_selectedRow is IEditable)
	            {
		            (_selectedRow as IEditable).StopEditElement();
	            }

	            _selectedRow = value;
                ElementsAddingCommandCollectionFiltered = ElementsAddingCommandCollection
                    .Where(command => command.AddingCommand.CanExecute(null)).ToObservableCollection();
                SelectedElementsAddingCommand = ElementsAddingCommandCollectionFiltered.FirstOrDefault();
                foreach (IElementAddingCommand elementAddingCommand in ElementsAddingCommandCollection)
	            {
		            (elementAddingCommand.AddingCommand as RelayCommand)?.RaiseCanExecuteChanged();
	            }


	            (EditDescriptionCommand as RelayCommand)?.RaiseCanExecuteChanged();
	            (AddSelectedElementAsResourceCommand as RelayCommand)?.RaiseCanExecuteChanged();
	            (EditElementCommand as RelayCommand)?.RaiseCanExecuteChanged();

	            (SetElementDownCommand as RelayCommand)?.RaiseCanExecuteChanged();
	            (SetElementUpCommand as RelayCommand)?.RaiseCanExecuteChanged();
	            (CopyElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
	            (PasteAsChildElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (CutElementCommand as RelayCommand)?.RaiseCanExecuteChanged();

                (ShowDependenciesCommand as RelayCommand)?.RaiseCanExecuteChanged();

	            RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConfigurationItemViewModel> RootConfigurationItemViewModels { get; set; }

        public ObservableCollection<IConfigurationItemViewModel> AllRows
        {
            get { return _allRows; }
            set
            {
                _allRows = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AddRootGroupElementCommand { get; set; }
        public ICommand AddRootElementCommand { get; set; }
        public ICommand EditElementCommand { get; set; }
        public ICommand DeleteElementCommand { get; set; }
        public ICommand ShowFormatterParametersCommand { get; set; }
        public ICommand SetElementUpCommand { get; set; }
        public ICommand SetElementDownCommand { get; set; }
        public ICommand OpenConfigurationSettingsCommand { get; set; }
        public ICommand OpenBasicValuesCommand { get;}
        public ICommand MigrateComplexPropertiesCommand { get;}

        public ICommand CopyElementCommand { get; }
        public ICommand CutElementCommand { get; }

        public ICommand OnSelectionChangedCommand { get; }

        public ICommand PasteAsChildElementCommand { get; }
        public ICommand AddSelectedElementAsResourceCommand { get; }
        public ICommand EditDescriptionCommand { get; }
        public ICommand DecreaseAddressCommand { get; }
        public ICommand IncreaseAddressCommand { get; }
        public ICommand TriggerAdditionalSettingsCommand { get; }

        public ushort AddressIteratorValue
        {
	        get => _addressIteratorValue;
	        set
	        {
		        _addressIteratorValue = value; 
				RaisePropertyChanged();
	        }
        }

        public ObservableCollection<IElementAddingCommand> ElementsAddingCommandCollectionFiltered
        {
            get => _elementsAddingCommandCollectionFiltered;
            set
            {
                _elementsAddingCommandCollectionFiltered = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<IElementAddingCommand> ElementsAddingCommandCollection { get; set; }

        public bool IsAdditionalSettingsOpened
        {
            get => _isAdditionalSettingsOpened;
            set
            {
                _isAdditionalSettingsOpened = value;
                RaisePropertyChanged();
            }
        }
        public bool IsMultiEditMode
        {
            get => _isMultiEditMode;
            set
            {
                _isMultiEditMode = value;
                RaisePropertyChanged();
            }
        }

        private bool CanExecuteShowFormatterParameters()
        {
            return SelectedRows.All(model=>model is IUshortFormattableEditorViewModel);
        }

        private void OnShowFormatterParametersExecute()
        {
            _formatterEditorFactory.EditFormatterByUser(SelectedRows.Cast<IUshortFormattableEditorViewModel>().ToList(),this.RootConfigurationItemViewModels.ToList());
        }

        private bool CanExecuteDeleteElement()
        {
            return SelectedRows.All(model => model is IDeletable);
        }

        private bool CanExecuteEditElement()
        {
            return (SelectedRow is IEditable);
        }

        //private bool CanExecuteAddChildElement()
        //{
        //    return (SelectedRow is IChildAddable);
        //}


        private void OnDeleteElementExecute()
        {
            if (_applicationGlobalCommands.AskUserToDeleteSelectedGlobal(this))
            {
                SelectedRows.ForEach(model =>
                {
                    if (!(model is IDeletable)) return;
                    DeleteHeirarchicalRow(model);
                });
              
            }
        }

        private void OnEditElementExecute()
        {
            if (SelectedRow is IEditable)
            {
                (SelectedRow as IEditable).StartEditElement();
            }
        }

        private bool CanExecuteSetElementUp()
        {
            if (SelectedRow == null) return false;
            if (RootConfigurationItemViewModels.Contains(SelectedRow))
            {
                return GetIsSetElementPossible(SelectedRow, true);
            }

            if (SelectedRow.Parent == null) return false;
            if (!(SelectedRow.Parent is IChildPositionChangeable)) return false;
            return (SelectedRow.Parent as IChildPositionChangeable)
                .GetIsSetElementPossible(SelectedRow, true);
        }

        private bool CanExecuteSetElementDown()

        {
            if (SelectedRow == null) return false;
            if (RootConfigurationItemViewModels.Contains(SelectedRow))
            {
                return GetIsSetElementPossible(SelectedRow, false);
            }

            if (SelectedRow.Parent == null) return false;
            if (!(SelectedRow.Parent is IChildPositionChangeable)) return false;
            return (SelectedRow.Parent as IChildPositionChangeable).GetIsSetElementPossible(SelectedRow,
                false);
        }

        private void OnSetElementUpExecute()
        {
            if (SelectedRow == null) return;
            IEditorConfigurationItemViewModel selectedRowBuffer = SelectedRow;
            bool isElementSetted = false;
            if (RootConfigurationItemViewModels.Contains(SelectedRow))
            {
                SetElement(SelectedRow, true);
            }
            else
            {
                if (SelectedRow.Parent == null) return;
                if (!(SelectedRow.Parent is IChildPositionChangeable)) return;
                SelectedRow.Checked?.Invoke(false);
                isElementSetted =
                    ((SelectedRow.Parent as IChildPositionChangeable).SetElement(SelectedRow, true));

                if (isElementSetted)
                {

                    SelectedRow?.Parent?.Checked?.Invoke(true);
                }
            }

            SelectedRow = selectedRowBuffer;

        }

        private void OnSetElementDownExecute()
        {
            if (SelectedRow == null) return;
            IEditorConfigurationItemViewModel selectedRowBuffer = SelectedRow;
            if (RootConfigurationItemViewModels.Contains(SelectedRow))
            {
                SetElement(SelectedRow, false);
            }
            else
            {
                if (SelectedRow.Parent == null) return;
                if (!(SelectedRow.Parent is IChildPositionChangeable)) return;
                SelectedRow.Checked?.Invoke(false);
                bool isElementSetted =
                    (SelectedRow.Parent as IChildPositionChangeable).SetElement(SelectedRow, false);

                if (isElementSetted)
                {
                    SelectedRow?.Parent?.Checked?.Invoke(true);
                }
            }

            SelectedRow = selectedRowBuffer;
        }


        private void Save()
        {
            if (SelectedRow is IEditable)
            {
                if ((SelectedRow as IEditable).IsInEditMode)
                {
                    (SelectedRow as IEditable).StopEditElement();
                }
            }
        }

        private void DeleteHeirarchicalRow(IEditorConfigurationItemViewModel configurationItemViewModel)
        {
            if (configurationItemViewModel.ChildStructItemViewModels != null)
            {
                if (configurationItemViewModel.ChildStructItemViewModels is IEnumerable)
                {
                    List<IConfigurationItemViewModel> itemsToDelete = new List<IConfigurationItemViewModel>();
                    itemsToDelete.AddRange(configurationItemViewModel.ChildStructItemViewModels);
                    foreach (IConfigurationItemViewModel item in itemsToDelete)
                    {
                        if (item is IEditorConfigurationItemViewModel)
                        {
                            DeleteHeirarchicalRow(item as IEditorConfigurationItemViewModel);
                        }
                    }
                }
            }


            if (configurationItemViewModel is IDeletable)
            {
                //проверка на корневой элемент (у него нет родителя и он состоит в списке корневых элементов)
                if (configurationItemViewModel.Parent == null)
                {
                    if (RootConfigurationItemViewModels.Contains(configurationItemViewModel))
                    {
                        RootConfigurationItemViewModels.Remove(configurationItemViewModel);
                        // this._deviceConfiguration.RootConfigurationItemList.Remove(configurationItemViewModel.Model as IConfigurationItem);
                    }
                }
                else
                {
                    ((IDeletable) configurationItemViewModel).DeleteElement();
                }
            }

            AllRows.Remove(configurationItemViewModel);
        }


        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;



        public string NameForUiKey => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION;

        public IDeviceFragment BuildDeviceFragment()
        {
            return ConfigurationFragmentFactory.CreateConfiguration(this);
        }


        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        public IFragmentSettingsViewModel FragmentSettingsViewModel { get; }

        public IBaseValuesViewModel BaseValuesViewModel
        {
            get => _baseValuesViewModel;
            set
            {
                _baseValuesViewModel = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand ShowDependenciesCommand
        {
            get;
        }

        public List<IEditorConfigurationItemViewModel> SelectedRows
        {
            get => _selectedRows;
            set
            {
                _selectedRows = value;
                RaisePropertyChanged();
            }
        }

        public object ShowFiltersCommand { get; }

        public IElementAddingCommand SelectedElementsAddingCommand
        {
            get => _selectedElementsAddingCommand;
            set
            {
                _selectedElementsAddingCommand = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand ImportPropertiesFromExcelTypeACommand
        {
            get;
        }

        public ICommand EditCommand
        {
            get;
        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IDeviceConfiguration deviceConfiguration)
            {
                RootConfigurationItemViewModels.Clear();
                AllRows.Clear();
                foreach (IConfigurationItem member in deviceConfiguration.RootConfigurationItemList)
                {
                    IEditorConfigurationItemViewModel itemEditorViewModel =
                        member.Accept(ConfigurationItemEditorViewModelFactory.Create());
                    RootConfigurationItemViewModels.Add(itemEditorViewModel);
                }
                FragmentSettingsViewModel.Model = deviceConfiguration.FragmentSettings;
                BaseValuesViewModel = _baseValuesFillHelper.CreateBaseValuesViewModel(deviceConfiguration.BaseValues);
            }
            InitRows(RootConfigurationItemViewModels, AllRows);
        }

        private void InitRows(IEnumerable<IConfigurationItemViewModel> configurationItemViewModels,
            ObservableCollection<IConfigurationItemViewModel> rows)
        {
            foreach (var configurationItemViewModel in configurationItemViewModels)
            {
                rows.Add(configurationItemViewModel);
            }
        }

        public bool GetIsSetElementPossible(IConfigurationItemViewModel element, bool isUp)
        {
            int indexOfElement = RootConfigurationItemViewModels.IndexOf(element);
            int itemsCount = RootConfigurationItemViewModels.Count;
            if (isUp)
            {
                return indexOfElement > 0;
            }
            else
            {
                return indexOfElement < itemsCount - 1;
            }
        }

        public bool SetElement(IConfigurationItemViewModel element, bool isUp)
        {
            int indexOfElement = RootConfigurationItemViewModels.IndexOf(element);
            int newIndexOfElement = isUp ? indexOfElement - 1 : indexOfElement + 1;
            if (element.IsChecked)
            {
                element.Checked?.Invoke(false);
            }

            IConfigurationItemViewModel replaceableElement = RootConfigurationItemViewModels[newIndexOfElement];
            if (replaceableElement.IsChecked)
            {
                replaceableElement.Checked?.Invoke(false);
            }

            RootConfigurationItemViewModels.Move(indexOfElement, newIndexOfElement);
            AllRows.Move(AllRows.IndexOf(replaceableElement), AllRows.IndexOf(element));
            return true;
        }

        public void RemoveChildItem(IEditorConfigurationItemViewModel configurationItemViewModelToRemove)
        {
            RootConfigurationItemViewModels.Remove(
                RootConfigurationItemViewModels.First((model => model == configurationItemViewModelToRemove)));
        }
    }
}