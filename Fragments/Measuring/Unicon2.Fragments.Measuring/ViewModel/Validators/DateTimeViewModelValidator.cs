using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Unicon2.Fragments.Measuring.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Measuring.ViewModel.Validators
{
    public class DateTimeViewModelValidator:AbstractValidator<DateTimeMeasuringElementViewModel>
    {
        public DateTimeViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.Date).Matches(@"\b(0[1-9]|[12][0-9]|3[01])\b[,]\b(0[1-9]|1[0-2])\b[,]\b(0[0-9]|[1-9][0-9])\b$$").WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR) +" ("+ localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.DATE_FORMAT)+")");
            RuleFor(model => model.Time).Matches(@"\d{2}[:]\d{2}[:]\d{2},\d{2}$").WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR)); ;

        }
        public override ValidationResult Validate(ValidationContext<DateTimeMeasuringElementViewModel> context)
        {
            if (!context.InstanceToValidate.IsInEditMode) return new ValidationResult();
            return base.Validate(context);
        }


    }
}
