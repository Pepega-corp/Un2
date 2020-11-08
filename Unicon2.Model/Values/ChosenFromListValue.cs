using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ChosenFromListValue : FormattedValueBase, IChosenFromListValue
    {
        private List<string> _availableItemsList;
        private bool _isDefaultValueInAvailable;

        public override string StrongName => nameof(ChosenFromListValue);

        public override string AsString()
        {
            return SelectedItem;
        }

        [JsonProperty]
        public List<string> AvailableItemsList
        {
            get { return _availableItemsList; }
        }

        [JsonProperty] public string SelectedItem { get; set; }

        public void InitList(IEnumerable<string> stringEnumerable)
        {
            _availableItemsList = new List<string>(stringEnumerable);

        }

        public void SetListDefaultValue(string defaultValue)
        {
            _availableItemsList.Insert(0, defaultValue);
            _isDefaultValueInAvailable = true;
            SelectedItem = defaultValue;
        }

        public bool IsDefaultValue(string str)
        {
            if (_isDefaultValueInAvailable)
            {
                if (string.Equals(str, _availableItemsList[0]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsDefaultValueSelected()
        {
            if (_isDefaultValueInAvailable)
            {
                if (string.Equals(SelectedItem, _availableItemsList[0]))
                {
                    return true;
                }
            }

            return false;
        }

        public override T Accept<T>(IValueVisitor<T> visitor)
        {
            return visitor.VisitChosenFromListValue(this);
        }
    }
}