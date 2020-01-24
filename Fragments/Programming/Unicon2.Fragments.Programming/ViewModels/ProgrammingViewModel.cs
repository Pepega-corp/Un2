using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Views;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ProgrammingViewModel : ViewModelBase, IProgrammingViewModel
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ILogicElementFactory _factory;
        private IProgrammModel _programmModel;
        
        #region Constructor
        public ProgrammingViewModel(IApplicationGlobalCommands globalCommands, ILogicElementFactory factory)
        {
            this._applicationGlobalCommands = globalCommands;
            this._factory = factory;

            this.SchemesCollection = new ObservableCollection<ISchemeTabViewModel>();

            this.NewSchemeCommand = new RelayCommand(this.CreateNewScheme);
            this.CloseTabCommand = new RelayCommand(this.CloseTab, this.CanCloseTab);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
            this.ZoomIncrementCommand = new RelayCommand(this.ZoomIncrement, this.CanZooming);
            this.ZoomDecrementCommand = new RelayCommand(this.ZoomDecrement, this.CanZooming);
        }

        #endregion

        #region Properties
        public string ProjectName
        {
            get => _programmModel.ProjectName;
            set
            {
                if (string.Equals(_programmModel.ProjectName, value, System.StringComparison.InvariantCultureIgnoreCase))
                    return;
                _programmModel.ProjectName = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedTabIndex { get; set; }

        public ObservableCollection<ISchemeTabViewModel> SchemesCollection { get; }

        #endregion


        #region NewSchemeCommand
        public ICommand NewSchemeCommand { get; }

        private void CreateNewScheme()
        {
            var schemeViewModel = new NewSchemeViewModel();
            this._applicationGlobalCommands.ShowWindowModal(()=>new NewSchemeView(), schemeViewModel);
            if (schemeViewModel.DialogResult == MessageDialogResult.Affirmative)
            {
                SchemeTabViewModel tabViewModel = new SchemeTabViewModel(schemeViewModel.SchemeName, schemeViewModel.SelectedSize);
                tabViewModel.CloseTabEvent += this.CloseTab;
                this.SchemesCollection.Add(tabViewModel);
            }
        }
        #endregion

        #region CloseTabCommand
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
        #endregion

        #region DeleteCommand
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
        #endregion

        #region ZoomCommand
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
        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => ProgrammingKeys.PROGRAMMING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            if (value is IProgrammModel model)
            {
                this._programmModel = model;

                foreach(var sceme in _programmModel.Schemes)
                {
                    SchemesCollection.Add(new SchemeTabViewModel(sceme));
                }
            }
        }

        private IProgrammModel GetModel()
        {
            _programmModel.Schemes = SchemesCollection.Select(sc => sc.Model).ToArray();

            return this._programmModel;
        }

        #endregion

        #region Implementation of IFragmentViewModel

        public string NameForUiKey => ProgrammingKeys.PROGRAMMING;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        #endregion

        
    }
}
