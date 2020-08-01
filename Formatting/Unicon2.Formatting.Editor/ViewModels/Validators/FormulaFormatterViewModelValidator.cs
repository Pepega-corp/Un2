using FluentValidation;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using org.mariuszgromada.math.mxparser;

namespace Unicon2.Formatting.Editor.ViewModels.Validators
{
    public class FormulaFormatterViewModelValidator : AbstractValidator<IFormulaFormatterViewModel>
    {
        public FormulaFormatterViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.FormulaString).NotEmpty()
                .WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages
                    .NULL_OR_EMPTY_MESSAGE));
            RuleFor(model => model.FormulaString).Must(((args, s) => IsFormulaStringValid(s, args)))
                .WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FORMAT_ERROR));
        }

        private bool IsFormulaStringValid(string formulaString, IFormulaFormatterViewModel arg)
        {
            try
            {
                Expression expression = new Expression(formulaString);
                expression.addArguments(new Argument("x", 1));


                int index = 1;

                foreach (var argument in (arg.ArgumentViewModels))
                {
                    expression.addArguments(new Argument("x" + index++, 1));
                }


                return !double.IsNaN(expression.calculate());
            }
            catch
            {
                return false;
            }

        }

    }
}