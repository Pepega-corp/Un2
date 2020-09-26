using FluentValidation;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Values.Validators
{
    public class RangeViewModelValidator : AbstractValidator<IRangeViewModel>
    {

        public RangeViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.RangeTo).Must((s =>
            {
              
                double x;
                return double.TryParse(s, out x);
            })).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
            RuleFor(model => model.RangeFrom).Must((s =>
            {
                double x;
                return double.TryParse(s, out x);
            })).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
            RuleFor(model => model.RangeTo).Must((fooArgs, rangeTo) =>
            {

                double rFrom;
                double rTo;
                if ((!double.TryParse(rangeTo, out rTo)) ||
                    (!double.TryParse(fooArgs.RangeFrom, out rFrom))) return false;
                return rTo >= rFrom;
            }).WithMessage(
                localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.INVALID_RANGE_ERROR));
            RuleFor(model => model.RangeFrom).Must((fooArgs, rangeFrom) =>
            {

                double rFrom;
                double rTo;
                if ((!double.TryParse(fooArgs.RangeTo, out rTo)) ||
                    (!double.TryParse(rangeFrom, out rFrom))) return false;
                return rTo >= rFrom;
            }).WithMessage(
                localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.INVALID_RANGE_ERROR));

        }

    }
}