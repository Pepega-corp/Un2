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
    public class EditableChosenFromListValueViewModel : EditableValueViewModelBase, IChosenFromListValueViewModel
    {
        private ObservableCollection<string> _availableItemsList;
        private string _selectedItemInitialValue;
        private IChosenFromListValue _chosenFromListValue;
        private object _model;

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             PresentationKeys.CHOSEN_FROM_LIST_VALUE_KEY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override void InitFromValue(IFormattedValue value)
        {
            _chosenFromListValue = value as IChosenFromListValue;
            InitList(_chosenFromListValue.AvailableItemsList);
            _selectedItemInitialValue = _chosenFromListValue.SelectedItem;
        }

        public override void SetBaseValueToCompare(ushort[] ushortsToCompare)
        {
            try
            {
                _selectedItemInitialValue = (_ushortsFormatter.Format(ushortsToCompare) as IChosenFromListValue)
                    .SelectedItem;
                SetIsChangedProperty(nameof(SelectedItem), _selectedItemInitialValue != SelectedItem);
            }
            catch (Exception e)
            {
                _selectedItemInitialValue=String.Empty;
                SetIsChangedProperty(nameof(SelectedItem), _selectedItemInitialValue != SelectedItem);
            }
        }

        public override object Model
        {
            get { return _chosenFromListValue; }
            set { _chosenFromListValue = value as IChosenFromListValue; }
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
                SetIsChangedProperty(nameof(SelectedItem), _selectedItemInitialValue!= value);
                _chosenFromListValue.UshortsValue = _ushortsFormatter?.FormatBack(_chosenFromListValue);
                ValueChangedAction?.Invoke(_chosenFromListValue.UshortsValue);
            }
        }

        public void InitList(IEnumerable<string> stringEnumerable)
        {
            _availableItemsList=new ObservableCollection<string>(stringEnumerable);
        }
    }
}