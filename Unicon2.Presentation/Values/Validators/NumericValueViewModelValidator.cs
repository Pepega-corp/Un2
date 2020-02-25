using System;
using FluentValidation;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Values.Editable;

namespace Unicon2.Presentation.Values.Validators
{
    public class NumericValueViewModelValidator : AbstractValidator<EditableNumericValueViewModel>
    {
        public NumericValueViewModelValidator(ILocalizerService localizerService, IUshortsFormatter ushortsFormatter)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor((model => model.NumValue)).Must((s =>
            {
                double x;
                return double.TryParse(s, out x);
            })).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
            RuleFor((model => model.NumValue)).Must((args, o) =>
            {
                INumericValue numericValue = args.GetValue() as INumericValue;
                var prev = numericValue.NumValue;
                try
                {
                    numericValue.NumValue = double.Parse(args.NumValue);
                    ushortsFormatter.FormatBack(numericValue);
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    numericValue.NumValue = prev;
                }

                return true;
            }).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
            RuleFor((model => model.NumValue)).Must((args, o) =>
            {
                if (!args.IsRangeEnabled) return true;
                if (args.Range == null) return true;
                double x;
                if (double.TryParse(o, out x))
                {
                    return args.Range.CheckValue(x);
                }

                return false;
            }).WithMessage(
                localizerService.GetLocalizedString(
                    ApplicationGlobalNames.StatusMessages.VALUE_OUT_OF_RANGE_MESSAGE_KEY));

        }
    }
}