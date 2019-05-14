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
        private IQuickMemoryAccessSetting _quickMemoryAccessSetting;
        private bool _isSettingEnabled;
        private IRangeViewModel _selectedRangeViewModel;

        public QuickAccessMemorySettingViewModel(Func<IRangeViewModel> rangeViewModelGettingFunc, ITypesContainer container)
        {
            this._rangeViewModelGettingFunc = rangeViewModelGettingFunc;
            this.AddRangeCommand = new RelayCommand(this.OnAddRangeExecute);
            this.DeleteRangeCommand = new RelayCommand(this.OnDeleteRangeExecute, this.CanExecuteDeleteRange);
            this.RangeViewModels = new ObservableCollection<IRangeViewModel>();
            this._quickMemoryAccessSetting = container.Resolve<IFragmentSetting>(ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING) as IQuickMemoryAccessSetting;
        }

        private bool CanExecuteDeleteRange()
        {
            return this.SelectedRangeViewModel != null;
        }

        private void OnDeleteRangeExecute()
        {
            this.RangeViewModels.Remove(this.SelectedRangeViewModel);
        }

        private void OnAddRangeExecute()
        {
            this.RangeViewModels.Add(this._rangeViewModelGettingFunc());
        }


        #region Implementation of IStronglyNamed

        public string StrongName => ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING +
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
            if (value is IQuickMemoryAccessSetting)
            {

                IQuickMemoryAccessSetting setting = value as IQuickMemoryAccessSetting;
                this._quickMemoryAccessSetting = setting;
                foreach (IRange range in setting.QuickAccessAddressRanges)
                {
                    IRangeViewModel rangeViewModel = this._rangeViewModelGettingFunc();
                    rangeViewModel.Model = range;
                    this.RangeViewModels.Add(rangeViewModel);
                }
                this.IsSettingEnabled = this._quickMemoryAccessSetting.IsSettingEnabled;
            }
        }

        private object GetModel()
        {
            this._quickMemoryAccessSetting.QuickAccessAddressRanges.Clear();

            foreach (IRangeViewModel rangeViewModel in this.RangeViewModels)
            {
                this._quickMemoryAccessSetting.QuickAccessAddressRanges.Add(rangeViewModel.Model as IRange);
            }
            this._quickMemoryAccessSetting.IsSettingEnabled = this.IsSettingEnabled;
            return this._quickMemoryAccessSetting;
        }

        #endregion

        #region Implementation of IConfigurationSettingViewModel

        public bool IsSettingEnabled
        {
            get { return this._isSettingEnabled; }
            set
            {
                this._isSettingEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of IQuickAccessMemorySettingViewModel

        public ObservableCollection<IRangeViewModel> RangeViewModels { get; set; }

        public IRangeViewModel SelectedRangeViewModel
        {
            get { return this._selectedRangeViewModel; }
            set
            {
                this._selectedRangeViewModel = value;
                (this.DeleteRangeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public ICommand AddRangeCommand { get; set; }
        public ICommand DeleteRangeCommand { get; set; }

        #endregion
    }
}