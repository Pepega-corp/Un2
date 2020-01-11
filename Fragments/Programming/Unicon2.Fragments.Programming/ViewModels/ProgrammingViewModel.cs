using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ProgrammingViewModel : ViewModelBase, IProgrammingViewModel
    {
        private readonly ITypesContainer _container;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private IProgrammModel _programmModel;

        public ProgrammingViewModel(ITypesContainer container, IApplicationGlobalCommands globalCommands, IElementLibraryViewModel library)
        {
            this._container = container;
            this._applicationGlobalCommands = globalCommands;

            this.SchemesCollection = new ObservableCollection<ISchemeTabViewModel>();
            this.ElementLibraryModel = library;

            this.NewSchemeCommand = new RelayCommand(this.CreateNewScheme);
            this.CloseTabCommand = new RelayCommand(this.CloseTab, this.CanCloseTab);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
            this.ZoomIncrementCommand = new RelayCommand(this.ZoomIncrement, this.CanZooming);
            this.ZoomDecrementCommand = new RelayCommand(this.ZoomDecrement, this.CanZooming);
        }

        public int SelectedTabIndex { get; set; }

        public ObservableCollection<ISchemeTabViewModel> SchemesCollection { get; }

        private IElementLibraryViewModel _libraryViewModel;
        public IElementLibraryViewModel ElementLibraryModel
        {
            get { return this._libraryViewModel; }
            set
            {
                if (this._libraryViewModel != null) return;
                this._libraryViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ICommand NewSchemeCommand { get; }

        private void CreateNewScheme()
        {
            //TODO сделать открытие модального окна с зарегистрированной вьюмоделью
            //if (info.DialogResult == DialogResultButtons.OK)
            //{
            //    SchemeTabViewModel tabViewModel = new SchemeTabViewModel(info.SchemeName, info.SchemeSize);
            //    tabViewModel.CloseTabEvent += this.CloseTab;
            //    this.SchemesCollection.Add(tabViewModel);
            //}
        }

        public ICommand CloseTabCommand { get; }

        private bool CanCloseTab()
        {
            return this.SchemesCollection.Count != 0;
        }

        private void CloseTab()
        {
            ISchemeTabViewModel tab = this.SchemesCollection[this.SelectedTabIndex];
            //TODO предложить сохранять схему или отменять закрытие

            tab.CloseTabEvent -= this.CloseTab;
            this.SchemesCollection.Remove(tab);
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

        public ICommand ZoomIncrementCommand { get; }

        private bool CanZooming()
        {
            return this.SchemesCollection.Count > 0;
        }

        private void ZoomIncrement()
        {
            this.SchemesCollection[this.SelectedTabIndex].ZoomIncrementCommand.Execute(this.SchemesCollection[this.SelectedTabIndex]);
        }
        
        public ICommand ZoomDecrementCommand { get; }

        private void ZoomDecrement()
        {
            this.SchemesCollection[this.SelectedTabIndex].ZoomDecrementCommand.Execute(this.SchemesCollection[this.SelectedTabIndex]);
        }

        public string StrongName => ProgrammingKeys.PROGRAMMING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return this._programmModel; }
            set { this._programmModel = value as IProgrammModel; }
        }

        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
    }
}
