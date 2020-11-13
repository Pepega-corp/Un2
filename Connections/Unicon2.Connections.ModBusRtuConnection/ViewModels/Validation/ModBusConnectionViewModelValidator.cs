using FluentValidation;
using FluentValidation.Results;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels.Validation
{
   public class ModBusConnectionViewModelValidator : AbstractValidator<IModBusConnectionViewModel>
    {
        public ModBusConnectionViewModelValidator(ILocalizerService localizerService)
        {
            RuleFor(model => string.IsNullOrEmpty(model.SelectedPort)).NotEqual(true).WithMessage(localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.SELECTED_DEVICE_NULL_MESSAGE));
            
        }

    }

}
