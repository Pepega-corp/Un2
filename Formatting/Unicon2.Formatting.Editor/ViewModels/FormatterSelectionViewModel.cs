using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Formatting.Editor.ViewModels.Helpers;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels
{

    public class FormatterSelectionViewModel : ViewModelBase, IFormatterSelectionViewModel
    {
        private readonly ITypesContainer _container;

        private readonly List<IUshortFormattableEditorViewModel> _ushortFormattableViewModel;

        //private readonly IUshortFormattable _ushortFormattable;
        private IUshortsFormatterViewModel _selectedUshortsFormatterViewModel;
        private ObservableCollection<IUshortsFormatterViewModel> _ushortsFormatterViewModels;
        private ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private string _currentResourceString;
        private bool _isFormatterFromResource;
        private bool _isFromBits;

        public FormatterSelectionViewModel(ITypesContainer container,
            List<IUshortFormattableEditorViewModel> ushortFormattableViewModels,
            List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            CurrentResourceString = null;
            _container = container;
            _ushortFormattableViewModel = ushortFormattableViewModels;

            _sharedResourcesGlobalViewModel = _container.Resolve<ISharedResourcesGlobalViewModel>();

            _ushortsFormatterViewModels = new ObservableCollection<IUshortsFormatterViewModel>();
            UshortsFormatterViewModels.AddCollection(_container.ResolveAll<IUshortsFormatterViewModel>());
            BitNumbersInWord = new ObservableCollection<IBitViewModel>();

            for (int i = 15; i >= 0; i--)
            {
                IBitViewModel bitViewModel = new BitViewModel(i, true);
                BitNumbersInWord.Add(bitViewModel);
            }

            IsBitsEditingEnabled = false;


            if (ushortFormattableViewModels.Count == 1)
            {
                var ushortFormattableViewModel = ushortFormattableViewModels.First();

                if (ushortFormattableViewModel is IBitsConfigViewModel bitsConfigViewModel)
                {
                    //BitNumbersInWord.AddCollection(
                    //    BitOwnershipHelper.CreateBitViewModelsWithOwnership(bitsConfigViewModel,
                    //        rootConfigurationItemViewModels));
                    bitsConfigViewModel.CopyBitsTo(this);
                    IsBitsEditingEnabled = true;
                }

                if (ushortFormattableViewModel.FormatterParametersViewModel != null)
                {
                    if (_sharedResourcesGlobalViewModel.CheckDeviceSharedResourcesContainsViewModel(
                        ushortFormattableViewModel.FormatterParametersViewModel.Name))
                    {
                        CurrentResourceString = ushortFormattableViewModel.FormatterParametersViewModel.Name;
                        _isFormatterFromResource = true;
                        ushortFormattableViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                            container.Resolve<IFormatterViewModelFactory>().CreateFormatterViewModel(
                                _sharedResourcesGlobalViewModel.GetResourceByName(_currentResourceString) as
                                    IUshortsFormatter).RelatedUshortsFormatterViewModel;
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

            _sharedResourcesGlobalViewModel.AddAsSharedResource(StaticContainer.Container.Resolve<ISaveFormatterService>().CreateUshortsParametersFormatter(formatterParametersViewModel));
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


                _ushortFormattableViewModel.ForEach(model =>model.FormatterParametersViewModel =
                    _container.Resolve<IFormatterViewModelFactory>().CreateFormatterViewModel(resourceUshortsFormatter));
            }
            else
            {
                _ushortFormattableViewModel.ForEach(model => model.FormatterParametersViewModel =
                    _container.Resolve<IFormatterParametersViewModel>());
                _ushortFormattableViewModel.ForEach(model => model.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                    SelectedUshortsFormatterViewModel);

            }


            if (_ushortFormattableViewModel.Count == 1)
            {
                var ushortFormattableViewModel = _ushortFormattableViewModel.First();

                if (ushortFormattableViewModel is IBitsConfigViewModel bitsConfigViewModel)
                {
                    this.CopyBitsTo(bitsConfigViewModel);
                    IsBitsEditingEnabled = true;
                }
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

        public string FormatterOwnersName =>string.Join(", ", _ushortFormattableViewModel.Select(model =>model.Name ).ToArray());
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

        public bool IsFromBits
        {
            get => _isFromBits;
            set
            {
                _isFromBits = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IBitViewModel> BitNumbersInWord { get; set; }
        public (ushort address, ushort numberOfPoints) GetAddressInfo()
        {
            throw new System.NotImplementedException();
        }

        public bool IsBitsEditingEnabled { get; set; }
    }
}