using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class ChosenFromListValueViewModel : FormattableValueViewModelBase,
        IChosenFromListValueViewModel
    {
        private ObservableCollection<string> _availableItemsList;
        private string _selectedItem;

        public override string StrongName => PresentationKeys.CHOSEN_FROM_LIST_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public ObservableCollection<string> AvailableItemsList => _availableItemsList;

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        public void InitList(IEnumerable<string> stringEnumerable)
        {
            _availableItemsList = new ObservableCollection<string>(stringEnumerable);
        }
    }
}