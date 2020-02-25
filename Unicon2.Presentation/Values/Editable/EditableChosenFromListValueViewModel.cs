using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values.Editable
{
    public class EditableChosenFromListValueViewModel : EditableValueViewModelBase<IChosenFromListValue>,
        IChosenFromListValueViewModel
    {
        private ObservableCollection<string> _availableItemsList;
        private string _selectedItemInitialValue;
        private IChosenFromListValue _chosenFromListValue;

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.CHOSEN_FROM_LIST_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override void InitFromValue(IChosenFromListValue value)
        {
            _chosenFromListValue = value;
            InitList(_chosenFromListValue.AvailableItemsList);
            _selectedItemInitialValue = _chosenFromListValue.SelectedItem;
        }

        public ObservableCollection<string> AvailableItemsList
        {
            get { return _availableItemsList; }
        }

        public string SelectedItem
        {
            get { return _chosenFromListValue.SelectedItem; }
            set
            {
                _chosenFromListValue.SelectedItem = value;
                RaisePropertyChanged();
                SetIsChangedProperty(nameof(SelectedItem), _selectedItemInitialValue != value);
            }
        }

        public void InitList(IEnumerable<string> stringEnumerable)
        {
            _availableItemsList = new ObservableCollection<string>(stringEnumerable);
        }

        public override IChosenFromListValue GetValue()
        {
            _chosenFromListValue.SelectedItem = SelectedItem;
            return _chosenFromListValue;
        }
    }
}