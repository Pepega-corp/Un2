using FluentValidation;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Connections.ModBusRtuConnection.ViewModels.Validation
{
   public class ModBusConnectionViewModelValidator : AbstractValidator<IModBusConnectionViewModel>
    {
        public ModBusConnectionViewModelValidator(ILocalizerService localizerService)
        {
          
        }
    }

}
