using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class ExportSelectionViewModel : ViewModelBase
    {
        public ExportSelectionViewModel()
        {
        }

        private Action _onSubmit;
        private IDeviceConfiguration _deviceConfiguration;
        private IEnumerable<SelectorForItemsGroup> _selectors;

        public void Initialize(Action onSubmit, IDeviceConfiguration deviceConfiguration)
        {
            _onSubmit = onSubmit;
            _deviceConfiguration = deviceConfiguration;
        }

        public IEnumerable<SelectorForItemsGroup> Selectors
        {
            get => _selectors;
            set => SetProperty(ref _selectors, value);
        }

        private void MapConfigItemsOnSelector(List<SelectorForItemsGroup> selectors,IItemsGroup itemsGroup)
        {

        }
    }

    public class SelectorForItemsGroup : ViewModelBase
    {
        private bool _isSelected;
        private IEnumerable<SelectorForItemsGroup> _selectors;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        public IEnumerable<SelectorForItemsGroup> Selectors
        {
            get => _selectors;
            set => SetProperty(ref _selectors, value);
        }

    public IItemsGroup RelatedItemsGroup { get; set; }
        
    }
}