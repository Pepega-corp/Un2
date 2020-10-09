using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
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


            if (ushortFormattableViewModel.FormatterParametersViewModel != null)
            {
                if (_sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesContainsViewModel(
                    ushortFormattableViewModel.FormatterParametersViewModel.Name))
                {
                    CurrentResourceString = ushortFormattableViewModel.FormatterParametersViewModel.Name;
                    _isFormatterFromResource = true;
                }

                var formatter =
                    _ushortsFormatterViewModels.FirstOrDefault(f =>
                        f.StrongName == ushortFormattableViewModel.FormatterParametersViewModel
                            .RelatedUshortsFormatterViewModel.StrongName);
                var existingIndex =
                    _ushortsFormatterViewModels.IndexOf(formatter);
                _ushortsFormatterViewModels.RemoveAt(existingIndex);
                _ushortsFormatterViewModels.Insert(existingIndex,
                    ushortFormattableViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel);
                SelectedUshortsFormatterViewModel = ushortFormattableViewModel.FormatterParametersViewModel
                    .RelatedUshortsFormatterViewModel;

            }


            CancelCommand = new RelayCommand<object>(OnCancelExecute);
            OkCommand = new RelayCommand<object>(OnOkExecute);
            ResetCommand = new RelayCommand(OnResetExecute);
            AddAsResourceCommand = new RelayCommand(OnAddAsResourceExecute);
            SelectFromResourcesCommand = new RelayCommand(OnSelectFromResourcesExecute);
        }

        private void OnSelectFromResourcesExecute()
        {
            var selectedFormatter =
                _sharedResourcesGlobalViewModel.OpenSharedResourcesForSelecting<IUshortsFormatter>();
            if (selectedFormatter == null) return;
            CurrentResourceString = selectedFormatter.Name;
            IsFormatterFromResource = true;
            SelectedUshortsFormatterViewModel = _container.Resolve<IFormatterViewModelFactory>()
                .CreateFormatterViewModel(selectedFormatter).RelatedUshortsFormatterViewModel;

        }

        private void OnAddAsResourceExecute()
        {
            if (_selectedUshortsFormatterViewModel == null) return;
            if (_isFormatterFromResource) return;
            var formatterParametersViewModel = _container.Resolve<IFormatterParametersViewModel>();
            formatterParametersViewModel.IsFromSharedResources = true;
            formatterParametersViewModel.RelatedUshortsFormatterViewModel = _selectedUshortsFormatterViewModel;

            _sharedResourcesGlobalViewModel.AddAsSharedResource(formatterParametersViewModel);
            CurrentResourceString = formatterParametersViewModel.Name;

            IsFormatterFromResource = true;
        }


        private void OnResetExecute()
        {
            UshortsFormatterViewModels.Clear();
            UshortsFormatterViewModels.AddCollection(_container.ResolveAll<IUshortsFormatterViewModel>());
            SelectedUshortsFormatterViewModel = null;
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

            if (CurrentResourceString != null)
            {
                ISaveFormatterService saveFormatterService = _container.Resolve<ISaveFormatterService>();

                IUshortsFormatter resourceUshortsFormatter =
                    saveFormatterService.CreateUshortsParametersFormatter(SelectedUshortsFormatterViewModel);
                resourceUshortsFormatter.Name = CurrentResourceString;
                _sharedResourcesGlobalViewModel.UpdateSharedResource(resourceUshortsFormatter);


                _ushortFormattableViewModel.FormatterParametersViewModel =
                    _container.Resolve<IFormatterViewModelFactory>().CreateFormatterViewModel(resourceUshortsFormatter);
            }
            else
            {
                _ushortFormattableViewModel.FormatterParametersViewModel =
                    _container.Resolve<IFormatterParametersViewModel>();
                _ushortFormattableViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                    SelectedUshortsFormatterViewModel;
            }

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