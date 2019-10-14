using FluentValidation;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Validators
{
   public class PropertyEditorEditorViewModelValidator:AbstractValidator<IPropertyEditorEditorViewModel>
    {
        public PropertyEditorEditorViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.Address).Must(s =>
            {
                ushort u;
                return ushort.TryParse(s, out u);
            }).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.ErrorMessages.FORMAT_ERROR));
            RuleFor(model => model.NumberOfPoints).Must(s =>
            {
                ushort u;
                return ushort.TryParse(s, out u);
            }).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.ErrorMessages.FORMAT_ERROR));
        }
    }
}
