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

        public abstract string StrongName { get; }


        public string Header
        {
            get { return this._header; }
            set
            {
                this._header = value;
                this.RaisePropertyChanged();
            }
        }

        public virtual void InitFromValue(IFormattedValue value)
        {
            Model = value;
        }

        public bool IsRangeEnabled { get; set; }
        public IRange Range { get; set; }

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

        public object Model { get; set; }
    }
}
