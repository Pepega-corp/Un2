using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.Factories;
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
            CurrentResourceString = null;
            _container = container;
            _ushortFormattableViewModel = ushortFormattableViewModel;

            _sharedResourcesGlobalViewModel = _container.Resolve<ISharedResourcesGlobalViewModel>();

            _ushortsFormatterViewModels = new ObservableCollection<IUshortsFormatterViewModel>();
            UshortsFormatterViewModels.AddCollection(_container.ResolveAll<IUshortsFormatterViewModel>());

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

            CancelCommand = new RelayCommand<object>(OnCancelExecute);
            OkCommand = new RelayCommand<object>(OnOkExecute);
            ResetCommand = new RelayCommand(OnResetExecute);
            AddAsResourceCommand = new RelayCommand(OnAddAsResourceExecute);
            SelectFromResourcesCommand = new RelayCommand(OnSelectFromResourcesExecute);
        }

        private void OnSelectFromResourcesExecute()
        {
            var selectedFormatterViewModel =
                _sharedResourcesGlobalViewModel.OpenSharedResourcesForSelecting<IUshortsFormatterViewModel>();
            if (selectedFormatterViewModel == null) return;
            CurrentResourceString = selectedFormatterViewModel.StrongName;
            IsFormatterFromResource = true;
            SelectedUshortsFormatterViewModel = selectedFormatterViewModel;
        }

        private void OnAddAsResourceExecute()
        {
            if (_selectedUshortsFormatterViewModel == null) return;

            //this._sharedResourcesViewModelFactory.AddSharedResource(
            //    this._selectedUshortsFormatterViewModel.GetFormatter());
            //this.CurrentResourceString = (this._selectedUshortsFormatterViewModel.Model as IUshortsFormatter).Name;
            IsFormatterFromResource = true;
        }


        private void OnResetExecute()
        {
            //   this.SelectedUshortsFormatterViewModel?.InitFromFormatter(null);
            IsFormatterFromResource = false;
            CurrentResourceString = null;
        }

        private void OnOkExecute(object obj)
        {
            if (SelectedUshortsFormatterViewModel == null) return;
            if (SelectedUshortsFormatterViewModel is IDynamicFormatterViewModel)
            {
                if (!((IDynamicFormatterViewModel) SelectedUshortsFormatterViewModel).IsValid) return;
            }

            _ushortFormattableViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                SelectedUshortsFormatterViewModel;
            // this._ushortFormattable.UshortsFormatter = this.SelectedUshortsFormatterViewModel.GetFormatter();
            (obj as Window)?.Close();
        }

        private void OnCancelExecute(object obj)
        {
            (obj as Window)?.Close();
        }

        public ObservableCollection<IUshortsFormatterViewModel> UshortsFormatterViewModels
        {
            get => _ushortsFormatterViewModels;
            set
            {
                _ushortsFormatterViewModels = value;
                RaisePropertyChanged();
            }
        }

        public string Name => _selectedUshortsFormatterViewModel?.StrongName;
        public ICommand CancelCommand { get; set; }
        public ICommand OkCommand { get; set; }

        public IUshortsFormatterViewModel SelectedUshortsFormatterViewModel
        {
            get { return _selectedUshortsFormatterViewModel; }
            set
            {
                _selectedUshortsFormatterViewModel = value;
                RaisePropertyChanged();
            }
        }

        public string FormatterOwnersName => _ushortFormattableViewModel.Name;
        public ICommand ResetCommand { get; }
        public ICommand AddAsResourceCommand { get; }
        public ICommand SelectFromResourcesCommand { get; }

        public string CurrentResourceString
        {
            get { return _currentResourceString; }
            set
            {
                _currentResourceString = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFormatterFromResource
        {
            get { return _isFormatterFromResource; }
            set
            {
                _isFormatterFromResource = value;
                RaisePropertyChanged();
            }
        }
    }
}