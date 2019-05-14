using System.Collections.Generic;

namespace Unicon2.Infrastructure.Values
{
    public interface IChosenFromListValue : IFormattedValue/*, IMeasurable*/
    {
        List<string> AvailableItemsList { get; }
        string SelectedItem { get; set; }
        void InitList(IEnumerable<string> stringEnumerable);
        void SetListDefaultValue(string defaultValue);
        bool IsDefaultValue(string str);
        bool IsDefaultValueSelected();

    }
}