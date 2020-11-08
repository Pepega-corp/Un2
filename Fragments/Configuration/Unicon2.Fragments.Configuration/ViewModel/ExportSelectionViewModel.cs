using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class ExportSelectionViewModel : ViewModelBase
    {
        public ExportSelectionViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmitExecute);
        }

        private IEnumerable<SelectorForItemsGroup> MapSelectorForItemsGroups(
            IEnumerable<SelectorForItemsGroupViewModel> selectorViewModels)
        {
            return selectorViewModels.Select((model =>
                new SelectorForItemsGroup(MapSelectorForItemsGroups(model.Selectors), model.RelatedItemsGroup,
                    model.IsSelected, IsDeviceDataPrinting, IsLocalDataPrinting)));
        }

        private void OnSubmitExecute()
        {
            _onSubmit(new ConfigurationExportSelector(IsDeviceDataPrinting, IsLocalDataPrinting,
                MapSelectorForItemsGroups(Selectors).ToList()));
        }

        private Action<ConfigurationExportSelector> _onSubmit;
        private IEnumerable<SelectorForItemsGroupViewModel> _selectors;
        private bool _isSavingInProcess;
        private bool _isDeviceDataPrinting;
        private bool _isLocalDataPrinting;

        public void Initialize(Action<ConfigurationExportSelector> onSubmit, IRuntimeConfigurationViewModel deviceConfiguration)
        {
            _onSubmit = onSubmit;
            List<SelectorForItemsGroupViewModel> selectors = new List<SelectorForItemsGroupViewModel>();
            MapConfigItemsOnSelector(selectors, ItemsGroupSelectorFunc(deviceConfiguration.RootConfigurationItemViewModels));
            Selectors = selectors;
            IsDeviceDataPrinting = true;
            IsLocalDataPrinting = true;
        }

        public IEnumerable<SelectorForItemsGroupViewModel> Selectors
        {
            get => _selectors;
            set => SetProperty(ref _selectors, value);
        }

        private IEnumerable<IRuntimeItemGroupViewModel> ItemsGroupSelectorFunc(IEnumerable<IRuntimeConfigurationItemViewModel> configurationItems)
        {
            return configurationItems.Where((item => item is IRuntimeItemGroupViewModel)).Cast<IRuntimeItemGroupViewModel>();
        }

        private void MapConfigItemsOnSelector(List<SelectorForItemsGroupViewModel> selectors,
            IEnumerable<IRuntimeItemGroupViewModel> itemsGroup)
        {
            selectors.AddRange(itemsGroup.Select((group =>
            {
                List<SelectorForItemsGroupViewModel> innerSelectors = new List<SelectorForItemsGroupViewModel>();
                var innerGroups = ItemsGroupSelectorFunc(group.ChildStructItemViewModels.Cast<IRuntimeConfigurationItemViewModel>()).ToArray();
                if (innerGroups.Any())
                {
                    MapConfigItemsOnSelector(innerSelectors, innerGroups);
                }

                return new SelectorForItemsGroupViewModel(innerSelectors, group);
            })));
        }

        public ICommand SubmitCommand { get; }

        public bool IsSavingInProcess
        {
            get => _isSavingInProcess;
            set => SetProperty(ref _isSavingInProcess, value);
        }

        public bool IsDeviceDataPrinting
        {
            get => _isDeviceDataPrinting;
            set => SetProperty(ref _isDeviceDataPrinting, value);
        }

        public bool IsLocalDataPrinting
        {
            get => _isLocalDataPrinting;
            set => SetProperty(ref _isLocalDataPrinting, value);
        }
    }

    public class SelectorForItemsGroupViewModel : ViewModelBase
    {
        public SelectorForItemsGroupViewModel(IEnumerable<SelectorForItemsGroupViewModel> selectors,
	        IRuntimeItemGroupViewModel relatedItemsGroup)
        {
            Selectors = selectors;
            RelatedItemsGroup = relatedItemsGroup;
            IsSelected = true;
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                Selectors.ForEach((model => model.IsSelected = value));
            }
        }

        public IEnumerable<SelectorForItemsGroupViewModel> Selectors { get; }

        public IRuntimeItemGroupViewModel RelatedItemsGroup { get; }

    }
}