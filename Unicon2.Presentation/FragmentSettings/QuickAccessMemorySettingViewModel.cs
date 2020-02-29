using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.FragmentSettings;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.FragmentSettings
{
    public class QuickAccessMemorySettingViewModel : ViewModelBase, IQuickAccessMemorySettingViewModel
    {
        private readonly Func<IRangeViewModel> _rangeViewModelGettingFunc;
        private readonly ITypesContainer _container;
        private IQuickMemoryAccessSetting _quickMemoryAccessSetting;
        private bool _isSettingEnabled;
        private IRangeViewModel _selectedRangeViewModel;

        public QuickAccessMemorySettingViewModel(Func<IRangeViewModel> rangeViewModelGettingFunc, ITypesContainer container)
        {
            _rangeViewModelGettingFunc = rangeViewModelGettingFunc;
            _container = container;
            AddRangeCommand = new RelayCommand(OnAddRangeExecute);
            DeleteRangeCommand = new RelayCommand(OnDeleteRangeExecute, CanExecuteDeleteRange);
            RangeViewModels = new ObservableCollection<IRangeViewModel>();
            _quickMemoryAccessSetting = container.Resolve<IFragmentSetting>(ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING) as IQuickMemoryAccessSetting;
        }

        private bool CanExecuteDeleteRange()
        {
            return SelectedRangeViewModel != null;
        }

        private void OnDeleteRangeExecute()
        {
            RangeViewModels.Remove(SelectedRangeViewModel);
        }

        private void OnAddRangeExecute()
        {
            RangeViewModels.Add(_rangeViewModelGettingFunc());
        }


        public string StrongName => ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return GetModel(); }
            set { SetModel(value); }
        }

        private void SetModel(object value)
        {
            if (value is IQuickMemoryAccessSetting)
            {

                IQuickMemoryAccessSetting setting = value as IQuickMemoryAccessSetting;
                _quickMemoryAccessSetting = setting;
                foreach (IRange range in setting.QuickAccessAddressRanges)
                {
                    IRangeViewModel rangeViewModel = _rangeViewModelGettingFunc();
                    rangeViewModel.RangeFrom = range.RangeFrom.ToString();
                    rangeViewModel.RangeTo = range.RangeTo.ToString();

                    RangeViewModels.Add(rangeViewModel);
                }
                IsSettingEnabled = _quickMemoryAccessSetting.IsSettingEnabled;
            }
        }

        private object GetModel()
        {
            _quickMemoryAccessSetting.QuickAccessAddressRanges.Clear();

            foreach (IRangeViewModel rangeViewModel in RangeViewModels)
            {
                var range=_container.Resolve<IRange>();
                range.RangeTo = double.Parse(rangeViewModel.RangeTo);
                range.RangeFrom = double.Parse(rangeViewModel.RangeFrom);

                _quickMemoryAccessSetting.QuickAccessAddressRanges.Add(range);
            }
            _quickMemoryAccessSetting.IsSettingEnabled = IsSettingEnabled;
            return _quickMemoryAccessSetting;
        }

        public bool IsSettingEnabled
        {
            get { return _isSettingEnabled; }
            set
            {
                _isSettingEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IRangeViewModel> RangeViewModels { get; set; }

        public IRangeViewModel SelectedRangeViewModel
        {
            get { return _selectedRangeViewModel; }
            set
            {
                _selectedRangeViewModel = value;
                (DeleteRangeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public ICommand AddRangeCommand { get; set; }
        public ICommand DeleteRangeCommand { get; set; }
    }
}