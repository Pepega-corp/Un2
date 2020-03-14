using System;
using FluentValidation;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Values.Editable;

namespace Unicon2.Presentation.Values.Validators
{
    public class NumericValueViewModelValidator : AbstractValidator<EditableNumericValueViewModel>
    {
        public NumericValueViewModelValidator()
        {
            var localizerService = StaticContainer.Container.Resolve<ILocalizerService>();
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor((model => model.NumValue)).Must((s =>
            {
                double x;
                return double.TryParse(s, out x);
            })).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
            RuleFor((model => model.NumValue)).Must((args, o) =>
            {
                if (!args.IsRangeEnabled) return true;
                if (args.Range == null) return true;
                return double.TryParse(o, out var x) && args.Range.CheckValue(x);
            }).WithMessage(
                localizerService.GetLocalizedString(
                    ApplicationGlobalNames.StatusMessages.VALUE_OUT_OF_RANGE_MESSAGE_KEY));

        }
    }
}