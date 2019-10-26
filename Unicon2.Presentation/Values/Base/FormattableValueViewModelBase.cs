using System;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Values.Base
{
    public abstract class FormattableValueViewModelBase : ValidatableBindableBase, IFormattedValueViewModel
    {
        private string _header;
        private string _measureUnit;
        private bool _isMeasureUnitEnabled;
        #region Implementation of IStronglyNamed

        public abstract string StrongName { get; }

        #endregion

        #region Implementation of IFormattedValueViewModel


        public string Header
        {
            get { return this._header; }
            set
            {
                this._header = value;
                this.RaisePropertyChanged();
            }
        }

        public abstract void InitFromValue(IFormattedValue value);

        public Action<object, object> FormattedValueChanged { get; set; }

        #endregion


        #region Implementation of IRangeable

        public bool IsRangeEnabled { get; set; }
        public IRange Range { get; set; }

        #endregion

        #region Implementation of IMeasurable

        public string MeasureUnit
        {
            get => _measureUnit;
            set => SetProperty(ref _measureUnit, value);
        }

        public bool IsMeasureUnitEnabled
        {
            get => _isMeasureUnitEnabled;
            set => SetProperty(ref _isMeasureUnitEnabled, value);
        }

        #endregion
    }
}
