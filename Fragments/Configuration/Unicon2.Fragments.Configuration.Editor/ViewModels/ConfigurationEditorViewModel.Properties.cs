using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public partial class ConfigurationEditorViewModel
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
        
        private Result<(List<IEditorConfigurationItemViewModel> item, bool isMove)> _bufferConfigurationItems =
            ResultUtils.Nothing;

        private ushort _addressIteratorValue;
        private bool _isAdditionalSettingsOpened;
        private bool _isMultiEditMode;
        private List<IEditorConfigurationItemViewModel> _selectedRows;
        private ObservableCollection<IElementAddingCommand> _elementsAddingCommandCollectionFiltered;
        private IElementAddingCommand _selectedElementsAddingCommand;
        private IBaseValuesViewModel _baseValuesViewModel;
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
        public ICommand OpenBasicValuesCommand { get; }
        public ICommand MigrateComplexPropertiesCommand { get; }
        public ICommand CopyElementCommand { get; }
        public ICommand CutElementCommand { get; }
        public ICommand OnSelectionChangedCommand { get; }
        public ICommand PasteAsChildElementCommand { get; }
        public ICommand AddSelectedElementAsResourceCommand { get; }
        public ICommand EditDescriptionCommand { get; }
        public ICommand DecreaseAddressCommand { get; }
        public ICommand IncreaseAddressCommand { get; }
        public ICommand TriggerAdditionalSettingsCommand { get; }
        public ICommand AddDependencyToManyProps { get; }

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

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public string NameForUiKey => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION;
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

        public ICommand ImportPropertiesFromExcelTypeACommand { get; }

        public ICommand EditCommand { get; }
        public SetIsFromBitsToManyPropsViewModel SetIsFromBitsToManyPropsViewModel { get; }
    }
}