using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IChosenFromListValueViewModel : IFormattedValueViewModel
    {
        ObservableCollection<string> AvailableItemsList { get; }
        string SelectedItem { get; set; }
        void InitList(IEnumerable<string> stringEnumerable);
        void SetValue(string value);
    }
}