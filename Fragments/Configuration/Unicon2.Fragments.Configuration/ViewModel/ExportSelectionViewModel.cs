using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
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
                    model.IsSelected)));
        }

        private void OnSubmitExecute()
        {
            _onSubmit(MapSelectorForItemsGroups(Selectors).ToList());
        }

        private Action<List<SelectorForItemsGroup>> _onSubmit;
        private IEnumerable<SelectorForItemsGroupViewModel> _selectors;
        private bool _isSavingInProcess;

        public void Initialize(Action<List<SelectorForItemsGroup>> onSubmit, IDeviceConfiguration deviceConfiguration)
        {
            _onSubmit = onSubmit;
            List<SelectorForItemsGroupViewModel> selectors = new List<SelectorForItemsGroupViewModel>();
            MapConfigItemsOnSelector(selectors, ItemsGroupSelectorFunc(deviceConfiguration.RootConfigurationItemList));
            Selectors = selectors;
        }

        public IEnumerable<SelectorForItemsGroupViewModel> Selectors
        {
            get => _selectors;
            set => SetProperty(ref _selectors, value);
        }

        private IEnumerable<IItemsGroup> ItemsGroupSelectorFunc(IEnumerable<IConfigurationItem> configurationItems)
        {
            return configurationItems.Where((item => item is IItemsGroup)).Cast<IItemsGroup>();
        }

        private void MapConfigItemsOnSelector(List<SelectorForItemsGroupViewModel> selectors,
            IEnumerable<IItemsGroup> itemsGroup)
        {
            selectors.AddRange(itemsGroup.Select((group =>
            {
                List<SelectorForItemsGroupViewModel> innerSelectors = new List<SelectorForItemsGroupViewModel>();
                var innerGroups = ItemsGroupSelectorFunc(group.ConfigurationItemList).ToArray();
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
    }

    public class SelectorForItemsGroupViewModel : ViewModelBase
    {
        public SelectorForItemsGroupViewModel(IEnumerable<SelectorForItemsGroupViewModel> selectors,
            IItemsGroup relatedItemsGroup)
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

        public IItemsGroup RelatedItemsGroup { get; }

    }
}