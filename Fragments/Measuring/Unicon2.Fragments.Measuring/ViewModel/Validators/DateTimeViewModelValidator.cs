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
            RuleFor(model => model.Date).Matches(@"\d{2}[,]\d{2}[,]\d{2}").WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
            RuleFor(model => model.Time).Matches(@"\d{2}[:]\d{2}[:]\d{2},\d{2}").WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR)); ;

        }
        public override ValidationResult Validate(ValidationContext<DateTimeMeasuringElementViewModel> context)
        {
            if (!context.InstanceToValidate.IsInEditMode) return new ValidationResult();
            return base.Validate(context);
        }


    }
}
