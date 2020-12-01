using FluentValidation;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.ModuleDeviceEditing.Interfaces;

namespace Unicon2.ModuleDeviceEditing.ViewModels.Validators
{
    public class DeviceEditingViewModelValidator : AbstractValidator<IDeviceEditingViewModel>
    {
        public DeviceEditingViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => model.SelectedDevice).NotNull()
                .WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.SELECTED_DEVICE_NULL_MESSAGE));
            RuleFor(model => model.SelectedDeviceConnection).NotNull()
                .WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.SELECTED_CONNECTION_NULL_MESSAGE));
            RuleFor(model => model.DeviceSignature).Must(s =>!string.IsNullOrWhiteSpace(s))
                .WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.NULL_OR_EMPTY_MESSAGE));
        }
    }
}