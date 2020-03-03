using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Validators;

namespace Unicon2.Presentation.Values
{
    public class RangeViewModel : ValidatableBindableBase, IRangeViewModel
    {
        private IRange _range;
        private readonly ILocalizerService _localizerService;
        private string _rangeFrom;
        private string _rangeTo;


        public RangeViewModel(IRange range, ILocalizerService localizerService)
        {
            _range = range;
            _localizerService = localizerService;
        }

        public string StrongName => ApplicationGlobalNames.ViewModelNames.RANGE_VIEW_MODEL_NAME;

        public object Model
        {
            get
            {
                SaveRange();
                return _range;
            }
            set
            {
                if (value == null) return;
                if (!(value is IRange))
                {
                    throw new ArgumentException();
                }

                SetRange(value as IRange);
            }
        }

        private void SaveRange()
        {
            double rangeTo;
            if (double.TryParse(_rangeTo, out rangeTo))
            {
                _range.RangeTo = rangeTo;
            }

            double rangeFrom;
            if (double.TryParse(_rangeFrom, out rangeFrom))
            {
                _range.RangeFrom = rangeFrom;
            }
        }

        private void SetRange(IRange range)
        {
            RangeFrom = range.RangeFrom.ToString();
            RangeTo = range.RangeTo.ToString();
        }

        protected override void OnValidate()
        {
            var res = new RangeViewModelValidator(_localizerService).Validate(this);
            SetValidationErrors(res);
        }

        public string RangeFrom
        {
            get { return _rangeFrom; }
            set
            {
                _rangeFrom = value;
                FireErrorsChanged(nameof(RangeTo));
                FireErrorsChanged();

                RaisePropertyChanged();
            }
        }

        public string RangeTo
        {
            get { return _rangeTo; }
            set
            {
                _rangeTo = value;
                FireErrorsChanged(nameof(RangeFrom));
                FireErrorsChanged();

                RaisePropertyChanged();

            }
        }

        public object Clone()
        {
            return new RangeViewModel(_range.Clone() as IRange, _localizerService);
        }
    }
}