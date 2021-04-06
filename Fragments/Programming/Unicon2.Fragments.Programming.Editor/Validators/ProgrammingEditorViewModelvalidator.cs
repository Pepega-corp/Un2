using FluentValidation;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Programming.Editor.Validators
{
    public class ProgrammingEditorViewModelValidator : AbstractValidator<IProgrammingEditorViewModel>
    {
        //public ProgrammingEditorViewModelValidator(ILocalizerService localizerService)
        //{
        //    RuleFor(model => model.MrNumber).Must(s => s.Length <= 3).WithMessage("");

        //    RuleFor(model => model.MrNumber).Must(s => int.TryParse(s, out var res)).WithMessage("Номер у");
        //}
    }
}