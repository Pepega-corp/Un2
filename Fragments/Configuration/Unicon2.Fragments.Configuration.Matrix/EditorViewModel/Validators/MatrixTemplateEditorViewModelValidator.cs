using System.Linq;
using FluentValidation;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Validators
{

    public class MatrixTemplateEditorViewModelValidator : AbstractValidator<MatrixTemplateEditorViewModel>
    {
        public MatrixTemplateEditorViewModelValidator(ILocalizerService localizerService)
        {

            RuleFor((model => model.VariableSignatureEditorViewModels))
                .Must(models => models.Select((model => model.Signature)).Distinct().Count() == models.Select((model => model.Signature)).Count()).WithMessage("repeat");
        }
    }
}

