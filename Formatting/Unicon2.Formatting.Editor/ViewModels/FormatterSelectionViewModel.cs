using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class FormatterSelectionViewModel : ViewModelBase, IFormatterSelectionViewModel
    {
        private readonly ITypesContainer _container;

        private readonly IUshortFormattableEditorViewModel _ushortFormattableViewModel;

        //private readonly IUshortFormattable _ushortFormattable;
        private IUshortsFormatterViewModel _selectedUshortsFormatterViewModel;
        private ObservableCollection<IUshortsFormatterViewModel> _ushortsFormatterViewModels;
        private ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private string _currentResourceString;
        private bool _isFormatterFromResource;

        public FormatterSelectionViewModel(ITypesContainer container,
            IUshortFormattableEditorViewModel ushortFormattableViewModel)
        {
            this.CurrentResourceString = null;
            this._container = container;
            _ushortFormattableViewModel = ushortFormattableViewModel;

            this._sharedResourcesGlobalViewModel = this._container.Resolve<ISharedResourcesGlobalViewModel>();

            this._ushortsFormatterViewModels = new ObservableCollection<IUshortsFormatterViewModel>();
            this.UshortsFormatterViewModels.AddCollection(this._container.ResolveAll<IUshortsFormatterViewModel>());

            //if ((currentUshortsFormatter != null) &&
            //    (this._sharedResourcesViewModelFactory.CheckDeviceSharedResourcesContainsElement(
            //        currentUshortsFormatter)))
            //{
            //    this.CurrentResourceString = currentUshortsFormatter.Name;
            //    this._isFormatterFromResource = true;
            //}

            //if (currentUshortsFormatter != null)
            //{
            //    //IUshortsFormatterViewModel formatter =
            //    //    this._ushortsFormatterViewModels.FirstOrDefault(f =>
            //    //        f.StrongName == currentUshortsFormatter.StrongName);
            //   // if (formatter != null)
            //   // {
            //       // formatter.InitFromFormatter(currentUshortsFormatter);
            //    //    this.SelectedUshortsFormatterViewModel = formatter;
            //  //  }
            //}

            this.CancelCommand = new RelayCommand<object>(this.OnCancelExecute);
            this.OkCommand = new RelayCommand<object>(this.OnOkExecute);
            this.ResetCommand = new RelayCommand(this.OnResetExecute);
            this.AddAsResourceCommand = new RelayCommand(this.OnAddAsResourceExecute);
            this.SelectFromResourcesCommand = new RelayCommand(this.OnSelectFromResourcesExecute);
        }

        private void OnSelectFromResourcesExecute()
        {

            var selectedFormatterViewModel =
                _sharedResourcesGlobalViewModel.OpenSharedResourcesForSelecting<IUshortsFormatterViewModel>();
            if (selectedFormatterViewModel == null) return;
            this.CurrentResourceString = selectedFormatterViewModel.StrongName;
            this.IsFormatterFromResource = true;
            this.SelectedUshortsFormatterViewModel = selectedFormatterViewModel as IUshortsFormatterViewModel;

        }

        private void OnAddAsResourceExecute()
        {
            if (this._selectedUshortsFormatterViewModel == null) return;

            //this._sharedResourcesViewModelFactory.AddSharedResource(
            //    this._selectedUshortsFormatterViewModel.GetFormatter());
            //this.CurrentResourceString = (this._selectedUshortsFormatterViewModel.Model as IUshortsFormatter).Name;
            this.IsFormatterFromResource = true;
        }


        private void OnResetExecute()
        {
            //   this.SelectedUshortsFormatterViewModel?.InitFromFormatter(null);
            this.IsFormatterFromResource = false;
            this.CurrentResourceString = null;
        }

        private void OnOkExecute(object obj)
        {
            if (this.SelectedUshortsFormatterViewModel == null) return;
            if (this.SelectedUshortsFormatterViewModel is IDynamicFormatterViewModel)
            {
                if (!((IDynamicFormatterViewModel) this.SelectedUshortsFormatterViewModel).IsValid) return;
            }

            _ushortFormattableViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel = SelectedUshortsFormatterViewModel;
            // this._ushortFormattable.UshortsFormatter = this.SelectedUshortsFormatterViewModel.GetFormatter();
            (obj as Window)?.Close();
        }

        private void OnCancelExecute(object obj)
        {
            (obj as Window)?.Close();
        }

        public ObservableCollection<IUshortsFormatterViewModel> UshortsFormatterViewModels
        {
            get => this._ushortsFormatterViewModels;
            set
            {
                this._ushortsFormatterViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public string Name => this._selectedUshortsFormatterViewModel?.StrongName;
        public ICommand CancelCommand { get; set; }
        public ICommand OkCommand { get; set; }

        public IUshortsFormatterViewModel SelectedUshortsFormatterViewModel
        {
            get { return this._selectedUshortsFormatterViewModel; }
            set
            {
                this._selectedUshortsFormatterViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public string FormatterOwnersName => this._ushortFormattableViewModel.Name;
        public ICommand ResetCommand { get; }
        public ICommand AddAsResourceCommand { get; }
        public ICommand SelectFromResourcesCommand { get; }

        public string CurrentResourceString
        {
            get { return this._currentResourceString; }
            set
            {
                this._currentResourceString = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsFormatterFromResource
        {
            get { return this._isFormatterFromResource; }
            set
            {
                this._isFormatterFromResource = value;
                this.RaisePropertyChanged();
            }
        }
    }
}