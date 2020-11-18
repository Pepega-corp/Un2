using System.IO.Ports;
using FluentValidation;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels.Validation
{
    public class ComPortConfigurationViewModelValidator : AbstractValidator<ComPortConfigurationViewModel>
    {
        public ComPortConfigurationViewModelValidator(ILocalizerService localizerService)
        {
        //    RuleFor(model => model.SelectedParity).Must(parity => parity != Parity.None).WithMessage(
        //        localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.NULL_OR_EMPTY_MESSAGE));
            RuleFor(model => model.SelectedStopBits).Must(sb => sb != StopBits.None).WithMessage(
                localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.NULL_OR_EMPTY_MESSAGE));
            RuleFor(model => model.WaitAnswer).Must(sb => sb >0).WithMessage(
                localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.MUST_BE_POSITIVE));
            RuleFor(model => model.WaitByte).Must(sb => sb >0).WithMessage(
                localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.MUST_BE_POSITIVE));
        }
    }
}