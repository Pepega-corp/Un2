﻿using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class NumericValueViewModel : FormattableValueViewModelBase<INumericValue>, INumericValueViewModel
    {
        private string _numValue;

        public override string StrongName => nameof(NumericValueViewModel);

        public override void InitFromValue(INumericValue value)
        {
            NumValue = value.NumValue.ToString();
            Header = value.Header;
        }

        public string NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                RaisePropertyChanged();
            }
        }
    }
}