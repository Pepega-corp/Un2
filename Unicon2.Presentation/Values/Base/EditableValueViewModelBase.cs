using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Values.Base
{
    public abstract class EditableValueViewModelBase : FormattableValueViewModelBase, IEditableValueViewModel
    {
        internal IUshortsFormatter _ushortsFormatter;

        public abstract override string StrongName { get; }
        
        public bool IsFormattedValueChanged => this._signaturedIsChangedPropertyDictionary.ContainsValue(true);
        public abstract void SetBaseValueToCompare(ushort[] ushortsToCompare);

        public void SetUshortFormatter(IUshortsFormatter ushortsFormatter)
        {
            this._ushortsFormatter = ushortsFormatter;
        }

        public Action<ushort[]> ValueChangedAction { get; set; }

        public bool IsEditEnabled
        {
            get { return this._isEditEnabled; }
            set
            {
                this._isEditEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private readonly Dictionary<string, bool> _signaturedIsChangedPropertyDictionary = new Dictionary<string, bool>();
        private bool _isEditEnabled;
  

        protected void SetIsChangedProperty(string propertyName, bool isChanged)
        {
            if (!this._signaturedIsChangedPropertyDictionary.ContainsKey(propertyName))
            {
                this._signaturedIsChangedPropertyDictionary.Add(propertyName, isChanged);
            }
            else
            {
                this._signaturedIsChangedPropertyDictionary[propertyName] = isChanged;
            }
            RaisePropertyChanged(nameof(this.IsFormattedValueChanged));
        }
        

        protected override void OnDisposing()
        {
            this.ValueChangedAction = null;
            base.OnDisposing();
        }
    }
}
