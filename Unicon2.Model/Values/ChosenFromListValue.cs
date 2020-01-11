using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class ChosenFromListValue:FormattedValueBase,IChosenFromListValue
    {
        private List<string> _availableItemsList;
        private bool _isDefaultValueInAvailable;

        public override string StrongName => nameof(ChosenFromListValue);
        public override string AsString()
        {
            return this.SelectedItem;
        }

        [DataMember]
        public List<string> AvailableItemsList
        {
            get { return this._availableItemsList; }
        }
        [DataMember]
        public string SelectedItem { get; set; }

        public void InitList(IEnumerable<string> stringEnumerable)
        {
            this._availableItemsList=new List<string>(stringEnumerable);
            
        }

        public void SetListDefaultValue(string defaultValue)
        {
            this._availableItemsList.Insert(0, defaultValue);
            this._isDefaultValueInAvailable = true;
            this.SelectedItem = defaultValue;
        }

        public bool IsDefaultValue(string str)
        {
            if (this._isDefaultValueInAvailable)
            {
                if (string.Equals(str, this._availableItemsList[0])) {
                    return true;
                }
            }
            return false;
        }

        public bool IsDefaultValueSelected()
        {
            if (this._isDefaultValueInAvailable)
            {
                if (string.Equals(this.SelectedItem, this._availableItemsList[0]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
